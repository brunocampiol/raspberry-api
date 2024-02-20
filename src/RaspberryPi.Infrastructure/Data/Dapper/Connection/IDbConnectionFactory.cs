using System.Data;

namespace RaspberryPi.Infrastructure.Data.Dapper.Connection
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}