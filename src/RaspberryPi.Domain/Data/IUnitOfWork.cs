namespace RaspberryPi.Domain.Data
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}