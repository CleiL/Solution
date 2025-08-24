using static Solution.Application.Dtos.Consultas.ConsultaDto;

namespace Solution.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<IEnumerable<AgendaSlotDto>> AgendaDoProfissionalAsync(Guid profissionalId, DateTime dia, CancellationToken ct);
        Task<ConsultaResponseDto> AgendarAsync(ConsultaCreateDto dto, CancellationToken ct);
    }
}
