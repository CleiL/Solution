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
    }
}
