using RaspberryPi.API.Contracts.Data;

namespace RaspberryPi.API.Repositories
{
    public interface ISqlLiteKeyValueRepository
    {
        Task<bool> CreateAsync(SqlLiteKeyValue keyValue);

        Task<SqlLiteKeyValue?> GetAsync(int id);

        Task<IEnumerable<SqlLiteKeyValue>> ListAsync();

        Task<bool> UpdateAsync(SqlLiteKeyValue keyValue);

        Task<bool> DeleteAsync(int id);
    }
}