using Dapper;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Dapper.Connection;

namespace RaspberryPi.Infrastructure.Data.Dapper.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        // TODO: fix this dapper repository
        // breaks the string to GUID when mapping in the list
        public DapperRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool Create(AspNetUser keyValue)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"INSERT INTO SqlLiteKeyValue (Id, Value, DateModified) 
                VALUES (@Id, @Value, @DateModified)",
                keyValue);
            return result == 1;
        }

        public AspNetUser? Get(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.QuerySingleOrDefault<AspNetUser>(
                "SELECT * FROM SqlLiteKeyValue WHERE Id = @Id LIMIT 1",
                new { Id = id });
        }

        public IEnumerable<AspNetUser> List()
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Query<AspNetUser>("SELECT * FROM AspNetUsers");
        }

        public bool Update(AspNetUser keyValue)
        {
            // TODO implement
            throw new NotImplementedException();

            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"UPDATE SqlLiteKeyValue SET Value = @Value, 
                                             DateModified = @DateModified
                WHERE Id = @Id",
                keyValue);
            return result == 1;
        }

        public bool Delete(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = connection.Execute(
                @"DELETE FROM AspNetUsers WHERE Id = @Id",
                new { Id = id });
            return result > 0;
        }

        public int Truncate()
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(@"DELETE FROM AspNetUsers");
        }
    }
}