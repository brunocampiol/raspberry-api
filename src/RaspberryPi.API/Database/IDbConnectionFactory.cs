using System.Data;

namespace RaspberryPi.API.Database
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}