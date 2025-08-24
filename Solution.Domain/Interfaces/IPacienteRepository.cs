using Solution.Domain.Entitites;

namespace Solution.Domain.Interfaces
{
    public interface IPacienteRepository
        : IBaseRepository<Paciente>
    {
        Task<bool> ExistsByCpfAsync(string cpf, Guid? excludeId = null, CancellationToken cancellationToken = default);
    }
}
