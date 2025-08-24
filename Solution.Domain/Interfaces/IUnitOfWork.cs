﻿using System.Data;

namespace Solution.Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IDbConnection Connection { get; }       
        IDbTransaction? Transaction { get; }    
        bool IsActive { get; }
        Task BeginAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
