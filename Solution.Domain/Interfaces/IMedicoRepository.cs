using Solution.Domain.Entitites;


namespace Solution.Domain.Interfaces
{
    public interface IMedicoRepository
        : IBaseRepository<Medico>
    {
        Task<bool> ExistsByCrmAsync(string crm, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
    }
}
