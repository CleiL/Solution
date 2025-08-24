using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application.Dtos.Paciente;
using Solution.Application.Services;
using Solution.Domain.Entitites;
using Solution.Domain.Interfaces;
using Xunit;

namespace Solution.Tests.Unit
{
    public class PacienteServiceTests
    {
        [Fact]
        public async Task Create_DeveCriarPaciente_quandoDadosValidos()
        {
            // arrange
            var logger = Mock.Of<ILogger<PacienteService>>();

            var repo = new Mock<IPacienteRepository>();
            var uow = new Mock<IUnitOfWork>();
            var uowFactory = new Mock<IUnitOfWorkFactory>();

            uowFactory.Setup(f => f.CreateAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(uow.Object);
            uow.Setup(x => x.BeginAsync(It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);
            uow.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            // agora a assinatura é (cpf, excludeId, ct)
            repo.Setup(r => r.ExistsByCpfAsync("12345678901", null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // repo retorna a própria entidade (o Service já define o Guid)
            repo.Setup(r => r.CreateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Paciente p, CancellationToken _) => p);

            var service = new PacienteService(repo.Object, logger, uowFactory.Object);

            var dto = new PacienteDto.PacienteCreateDto
            {
                Nome = "Maria da Silva",
                CPF = "12345678901",
                Email = "maria@exemplo.com",
                Phone = "11999999999"
            };

            // act
            var result = await service.CreateAsync(dto, CancellationToken.None);

            // assert
            result.Should().NotBeNull();
            result.PacienteId.Should().NotBe(Guid.Empty);
            result.Nome.Should().Be("Maria da Silva");

            repo.Verify(r => r.ExistsByCpfAsync("12345678901", null, It.IsAny<CancellationToken>()), Times.Once);
            repo.Verify(r => r.CreateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()), Times.Once);

            uowFactory.Verify(f => f.CreateAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Create_DeveFalhar_QuandoCpfDuplicado()
        {
            // arrange
            var logger = Mock.Of<ILogger<PacienteService>>();

            var repo = new Mock<IPacienteRepository>();
            var uow = new Mock<IUnitOfWork>();
            var uowFactory = new Mock<IUnitOfWorkFactory>();

            uowFactory.Setup(f => f.CreateAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(uow.Object);
            uow.Setup(x => x.BeginAsync(It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);
            uow.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            repo.Setup(r => r.ExistsByCpfAsync("12345678901", null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new PacienteService(repo.Object, logger, uowFactory.Object);

            var dto = new PacienteDto.PacienteCreateDto
            {
                Nome = "João",
                CPF = "12345678901"
            };

            // act
            Func<Task> act = async () => await service.CreateAsync(dto, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*CPF*já cadastrado*");

            repo.Verify(r => r.CreateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()), Times.Never);

            uowFactory.Verify(f => f.CreateAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        public static class ServiceBuilder
        {
            public static (PacienteService svc, Mock<IPacienteRepository> repo, Mock<IUnitOfWork> uow, Mock<IUnitOfWorkFactory> factory)
                Build()
            {
                var repo = new Mock<IPacienteRepository>();
                var uow = new Mock<IUnitOfWork>();
                var fac = new Mock<IUnitOfWorkFactory>();

                fac.Setup(f => f.CreateAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(uow.Object);
                uow.Setup(x => x.BeginAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
                uow.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
                uow.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

                var logger = Mock.Of<ILogger<PacienteService>>();
                var svc = new PacienteService(repo.Object, logger, fac.Object);
                return (svc, repo, uow, fac);
            }
        }

        [Fact]
        public async Task Update_DeveAtualizar_QuandoDadosValidos()
        {
            var (svc, repo, uow, _) = ServiceBuilder.Build();
            var id = Guid.NewGuid();
            var existente = new Paciente { PacienteId = id, Nome = "A", CPF = "111", Email = "a@a.com", Phone = "1" };

            repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existente);
            repo.Setup(r => r.ExistsByCpfAsync("222", id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Paciente p, CancellationToken _) => p);

            var dto = new PacienteDto.PacienteUpdateDto { PacienteId = id, Nome = "B", CPF = "222", Email = "b@b.com", Phone = "2" };
            var resp = await svc.UpdateAsync(dto, CancellationToken.None);

            resp.Nome.Should().Be("B");
            resp.CPF.Should().Be("222");
            repo.Verify(r => r.UpdateAsync(It.Is<Paciente>(p => p.PacienteId == id), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_DeveFalhar_QuandoCpfDuplicado()
        {
            var (svc, repo, uow, _) = ServiceBuilder.Build();
            var id = Guid.NewGuid();

            repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paciente { PacienteId = id, Nome = "A", CPF = "111" });
            repo.Setup(r => r.ExistsByCpfAsync("222", id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var dto = new PacienteDto.PacienteUpdateDto { PacienteId = id, CPF = "222" };
            var act = async () => await svc.UpdateAsync(dto, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("*CPF*já cadastrado*");

            repo.Verify(r => r.UpdateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()), Times.Never);
            uow.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_DeveCommitar_QuandoTrue()
        {
            var (svc, repo, uow, _) = ServiceBuilder.Build();
            var id = Guid.NewGuid();
            repo.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var ok = await svc.DeleteAsync(id, CancellationToken.None);

            ok.Should().BeTrue();
            uow.Verify(x => x.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_DeveRetornarDto_QuandoExiste()
        {
            var (svc, repo, uow, _) = ServiceBuilder.Build();
            var id = Guid.NewGuid();
            repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paciente { PacienteId = id, Nome = "X", CPF = "999" });

            var dto = await svc.GetByIdAsync(id, CancellationToken.None);

            dto.Should().NotBeNull();
            uow.Verify(x => x.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }



    }
}
