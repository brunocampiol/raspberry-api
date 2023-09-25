using System.Data;

namespace RaspberryPi.Infrastructure.Data.Connection
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}