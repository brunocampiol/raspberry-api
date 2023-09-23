namespace RaspberryPi.API.Data
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}