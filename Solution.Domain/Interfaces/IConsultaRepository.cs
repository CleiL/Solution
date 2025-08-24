using Solution.Domain.Entitites;

namespace Solution.Domain.Interfaces
{
    public interface IConsultaRepository
    {
        Task<bool> ExisteDoProfissionalNoHorario(Guid medicoId, DateTime dataHora, CancellationToken ct);
        Task<bool> PacienteJaTemNoDiaComProfissional(Guid pacienteId, Guid profissionalId, DateOnly dia, CancellationToken ct);
        Task CreateAsync(Consulta entity, CancellationToken ct);
        Task<IEnumerable<DateTime>> ObterHorariosOcupadosDoProfissional(Guid medicoId, DateOnly dia, CancellationToken ct);
    }
}
