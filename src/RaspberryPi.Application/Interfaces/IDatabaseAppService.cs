using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Application.Interfaces;

public interface IDatabaseAppService
{
    Task<string> GenerateDatabaseBackupAsJsonStringAsync();
    Task<int> ImportDatabaseBackupAsync(DbBackupDto backup);
}