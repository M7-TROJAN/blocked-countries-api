using A_Tecchnologies_Assignment.Models;

namespace A_Tecchnologies_Assignment.Repositories;

public interface IBlockedRepository
{
    // Manage blocked countries
    bool AddBlockedCountry(BlockedCountry country);
    bool AddOrUpdateBlockedCountry(BlockedCountry country);
    bool RemoveBlockedCountry(string countryCode);
    IEnumerable<BlockedCountry> GetBlockedCountries();
    BlockedCountry? GetCountryByCode(string countryCode);

    // Manage blocked attempts logs
    void AddBlockedAttempt(BlockedAttemptLog log);
    IEnumerable<BlockedAttemptLog> GetBlockedAttempts();
}