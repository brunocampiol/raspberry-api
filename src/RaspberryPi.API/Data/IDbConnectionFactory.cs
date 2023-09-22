using System.Data;

namespace RaspberryPi.API.Data
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}