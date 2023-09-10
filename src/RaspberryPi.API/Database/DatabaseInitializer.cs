using Dapper;

namespace RaspberryPi.API.Database
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS SqlLiteKeyValue (
                                            Id INTEGER PRIMARY KEY,
                                            Value TEXT NOT NULL,
                                            DateModified TEXT NOT NULL
                                           )");
        }
    }
}