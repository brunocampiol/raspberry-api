using Dapper;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Data.Dapper.Connection;

namespace RaspberryPi.Infrastructure.Data.Dapper.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        // TODO: migrate to use same tables from EF Core
        public DapperRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool Create(SqlLiteKeyValue keyValue)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"INSERT INTO SqlLiteKeyValue (Id, Value, DateModified) 
                VALUES (@Id, @Value, @DateModified)",
                keyValue);
            return result == 1;
        }

        public SqlLiteKeyValue? Get(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.QuerySingleOrDefault<SqlLiteKeyValue>(
                "SELECT * FROM SqlLiteKeyValue WHERE Id = @Id LIMIT 1",
                new { Id = id });
        }

        public IEnumerable<SqlLiteKeyValue> List()
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Query<SqlLiteKeyValue>("SELECT * FROM SqlLiteKeyValue");
        }

        public bool Update(SqlLiteKeyValue keyValue)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"UPDATE SqlLiteKeyValue SET Value = @Value, 
                                             DateModified = @DateModified
                WHERE Id = @Id",
                keyValue);
            return result == 1;
        }

        public bool Delete(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"DELETE FROM SqlLiteKeyValue WHERE Id = @Id",
                new { Id = id });
            return result > 0;
        }

        public int Truncate()
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(@"DELETE FROM SqlLiteKeyValue");
        }
    }
}