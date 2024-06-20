﻿using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces
{
    public interface IFactAppService
    {
        Task<Fact?> GetFirstOrDefaultFactFromDatabaseAsync();
        Task<FactResponse> GetRawRandomFactAsync();
        Task<FactResponse> SaveFactAndComputeHashAsync();
        Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync();
        Task<long> CountAllDatabaseFacts();
        Task<int> ImportBackupAsync(IEnumerable<Fact> facts);
    }
}