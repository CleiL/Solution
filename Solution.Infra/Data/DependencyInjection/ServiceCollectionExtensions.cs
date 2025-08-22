using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solution.Infra.Data.Context;


namespace Solution.Infra.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraData(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<DbOptions>(cfg.GetSection("Database"));

            services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();

            return services;
        }
    }
}
    