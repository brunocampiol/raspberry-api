namespace RaspberryPi.Domain.Core
{
    public interface IUnitOfWork
    {
        bool Commit();
        Task<bool> CommitAsync();
    }
}