using Dapper;
using Solution.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Infra.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IUnitOfWork Uow;

        protected BaseRepository(IUnitOfWork uow) => Uow = uow;

        protected IDbConnection Conn => Uow.Connection;
        protected IDbTransaction? Tx => Uow.Transaction;

        protected Task<int> ExecAsync(string sql, object? p = null, CancellationToken ct = default)
            => Conn.ExecuteAsync(new CommandDefinition(sql, p, Tx, cancellationToken: ct));

        protected Task<IEnumerable<T>> QueryAsync<T>(string sql, object? p = null, CancellationToken ct = default)
            => Conn.QueryAsync<T>(new CommandDefinition(sql, p, Tx, cancellationToken: ct));

        protected Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? p = null, CancellationToken ct = default)
            => Conn.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, p, Tx, cancellationToken: ct));
    }
}
