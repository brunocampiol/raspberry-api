using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public interface ICommentRepository
    {
        Task<bool> CreateAsync(Comment user);

        Task<Comment?> GetAsync(Guid id);

        Task<IEnumerable<Comment>> ListAsync();

        Task<bool> UpdateAsync(Comment user);

        Task<bool> DeleteAsync(Guid id);
    }
}
