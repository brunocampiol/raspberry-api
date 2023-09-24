using System.Data;

namespace RaspberryPi.Domain.Data
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}