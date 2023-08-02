using RaspberryPi.API.Contracts.Data;

namespace RaspberryPi.API.Repositories
{
    public interface ICommentRepository
    {
        Task<bool> CreateAsync(Comment user);

        Task<Comment?> GetAsync(string id);

        Task<IEnumerable<Comment>> GetAllAsync();

        Task<bool> UpdateAsync(Comment user);

        Task<bool> DeleteAsync(string id);
    }
}
