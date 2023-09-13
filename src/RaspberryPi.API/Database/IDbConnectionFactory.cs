using System.Data;

namespace RaspberryPi.API.Database
{
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();
    }
}