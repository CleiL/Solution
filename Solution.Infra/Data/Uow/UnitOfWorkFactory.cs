using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Solution.Domain.Interfaces;

namespace Solution.Infra.Data.Uow
{
    public sealed class UnitOfWorkFactory
        (
            IServiceProvider sp,
            ILogger<UnitOfWorkFactory> logger
        ) : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _sp = sp;
        private readonly ILogger<UnitOfWorkFactory> _logger = logger;

        public Task<IUnitOfWork> CreateAsync(CancellationToken ct = default)
        {
            var uow = _sp.GetRequiredService<IUnitOfWork>(); // scoped
            _logger.LogDebug("UoW criado via Factory");
            return Task.FromResult(uow);
        }
    }
}
