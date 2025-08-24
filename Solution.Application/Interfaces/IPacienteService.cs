using static Solution.Application.Dtos.Paciente.PacienteDto;

namespace Solution.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<PacienteResponseDto> CreateAsync(PacienteCreateDto entity, CancellationToken cancellationToken = default);
        Task<PacienteResponseDto> UpdateAsync(PacienteUpdateDto entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PacienteResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PacienteResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
