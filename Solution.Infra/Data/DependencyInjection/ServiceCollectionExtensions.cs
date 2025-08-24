using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solution.Application.Interfaces;
using Solution.Application.Services;
using Solution.Domain.Interfaces;
using Solution.Infra.Data.Context;
using Solution.Infra.Data.Uow;
using Solution.Infra.Repositories;


namespace Solution.Infra.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraData(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<DbOptions>(cfg.GetSection("Database"));

            services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IPacienteService, PacienteService>();

            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IMedicoService, MedicoService>();

            services.AddScoped<IConsultaRepository, ConsultaRepository>();

            return services;
        }
    }
}
    