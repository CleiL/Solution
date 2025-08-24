﻿using Dapper;
using Microsoft.Extensions.Logging;
using Solution.Domain.Entitites;
using Solution.Domain.Interfaces;

namespace Solution.Infra.Repositories
{
    public class ConsultaRepository
        (
            ILogger<ConsultaRepository> logger,
            IUnitOfWork uow
        )
        : BaseRepository(uow), IConsultaRepository
    {
        private readonly ILogger<ConsultaRepository> _logger = logger;

        public async Task CreateAsync(Consulta entity, CancellationToken ct)
        {
            using (_logger.BeginScope(new Dictionary<string, object?>
            {
                ["Flow"] = "Consulta.Create",
                ["ConsultaId"] = entity.ConsultaId
            }))
            {
                const string sql = """
                    insert into Consultas (ConsultaId, MedicoId, PacienteId, DataHora)
                    values (@ConsultaId, @MedicoId, @PacienteId, @DataHora)
                    """;
                _logger.LogDebug("Executando inserte na tabela");
                await Conn.ExecuteAsync(new CommandDefinition(sql, entity, Tx, cancellationToken: ct));
                _logger.LogInformation("Registrado com sucesso");
            }
        }

        public Task<bool> ExisteDoProfissionalNoHorario(Guid medicoId, DateTime dataHora, CancellationToken ct)
        {
            using (_logger.BeginScope(new Dictionary<string, object?>
            {
                ["Flow"] = "Consulta.ExisteDoProfissionalNoHorario",
                ["MedicoId"] = medicoId,
                ["DataHora"] = dataHora
            }))
            {
                const string sql = """
                    select case when exists (
                        select 1
                        from Consultas
                        where MedicoId = @medicoId and DataHora = @dataHora
                    ) then 1 else 0 end
                    """;
                _logger.LogDebug("Consultando na tabela");
                return Conn.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(sql, new { medicoId, dataHora }, Tx, cancellationToken: ct));
            }
        }

        public Task<IEnumerable<DateTime>> ObterHorariosOcupadosDoProfissional(Guid medicoId, DateOnly dia, CancellationToken ct)
        {
            using (_logger.BeginScope(new Dictionary<string, object?>
            {
                ["Flow"] = "Consulta.ObterHorariosOcupadosDoProfissional",
                ["MedicoId"] = medicoId,
                ["Dia"] = dia
            }))
            {
                var start = dia.ToDateTime(TimeOnly.MinValue);
                var end = start.AddDays(1);

                const string sql = """
                    select DataHora
                    from Consultas
                    where MedicoId = @medicoId and DataHora >= @start and DataHora < @end
                    order by DataHora
                    """;

                return Conn.QueryAsync<DateTime>(
                    new CommandDefinition(sql, new { medicoId, start, end }, Tx, cancellationToken: ct));
            }
        }

        public Task<bool> PacienteJaTemNoDiaComProfissional(Guid pacienteId, Guid profissionalId, DateOnly dia, CancellationToken ct)
        {
            using (_logger.BeginScope(new Dictionary<string, object?>
            {
                ["Flow"] = "Consulta.PacienteJaTemNoDiaComProfissional",
                ["PacienteId"] = pacienteId,
                ["ProfissionalId"] = profissionalId,
                ["Dia"] = dia
            }))
            {
                var start = dia.ToDateTime(TimeOnly.MinValue);
                var end = start.AddDays(1);

                const string sql = """
                select case when exists (
                    select 1
                    from Consultas
                    where PacienteId = @pacienteId 
                      and MedicoId   = @profissionalId 
                      and DataHora  >= @start 
                      and DataHora  <  @end
                ) then 1 else 0 end
                """;

                return Conn.ExecuteScalarAsync<bool>(
                    new CommandDefinition(sql, new { pacienteId, profissionalId, start, end }, Tx, cancellationToken: ct));
            }
        }
    }
}
