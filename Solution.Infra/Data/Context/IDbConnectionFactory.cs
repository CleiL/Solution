using System.Data;

namespace Solution.Infra.Data.Context
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> Create(CancellationToken cancellationToken = default);
    }
}
