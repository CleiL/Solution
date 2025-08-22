using System.Data;

namespace Solution.Infra.Data.Context
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}
