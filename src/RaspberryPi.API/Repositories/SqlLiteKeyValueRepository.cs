using Dapper;
using Microsoft.AspNetCore.Connections;
using RaspberryPi.API.Contracts.Data;
using RaspberryPi.API.Database;

namespace RaspberryPi.API.Repositories
{
    public class SqlLiteKeyValueRepository : ISqlLiteKeyValueRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlLiteKeyValueRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<bool> CreateAsync(SqlLiteKeyValue keyValue)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                @"INSERT INTO SqlLiteKeyValue (Id, Value, DateModified) 
                VALUES (@Id, @Value, @DateModified)",
                keyValue);
            return result == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                @"DELETE FROM SqlLiteKeyValue WHERE Id = @Id",
                new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<SqlLiteKeyValue>> ListAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<SqlLiteKeyValue>("SELECT * FROM SqlLiteKeyValue");
        }

        public async Task<SqlLiteKeyValue?> GetAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<SqlLiteKeyValue>(
                "SELECT * FROM SqlLiteKeyValue WHERE Id = @Id LIMIT 1",
                new { Id = id });
        }

        public async Task<bool> UpdateAsync(SqlLiteKeyValue keyValue)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                @"UPDATE SqlLiteKeyValue SET Value = @Value, 
                                             DateModified = @DateModified
                WHERE Id = @Id",
                keyValue);
            return result == 1;
        }
    }
}
