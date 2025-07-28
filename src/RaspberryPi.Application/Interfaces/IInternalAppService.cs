using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Application.Interfaces;

public interface IInternalAppService
{
    Task<string> GenerateDatabaseBackupAsJsonStringAsync();
    Task<int> ImportDatabaseBackupAsync(DbBackupDto backup);
}