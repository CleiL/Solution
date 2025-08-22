using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data;

namespace Solution.Infra.Data.Context
{
    public class SqliteConnectionFactory
        : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(IOptions<DbOptions> opts)
        {
            _connectionString = opts.Value.ConnectionString;
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Database:ConnectionString não configurada.");
        }

        public IDbConnection Create()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = """
                PRAGMA foreign_keys = ON; -- aplica o  Fks
                PRAGMA journal_mode = WAL; -- melhora a concorrência leitura/escrita
                PRAGMA synchronous = NORMAL;          
                PRAGMA busy_timeout = 3000; -- aguarda 3 segundos se o banco estiver ocupado
            """;
            command.ExecuteNonQuery();
            
            return connection;
        }
    }
}
