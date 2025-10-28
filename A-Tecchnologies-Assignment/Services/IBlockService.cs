using A_Tecchnologies_Assignment.DTOs;
using A_Tecchnologies_Assignment.Models;

namespace A_Tecchnologies_Assignment.Services;

public interface IBlockService
{
    bool BlockCountry(BlockCountryRequest request);
    bool UnblockCountry(string countryCode);
    PagedResult<BlockedCountry> GetBlockedCountries(int page, int pageSize, string? search = null);
    bool TemporarilyBlockCountry(TemporalBlockRequest request);
    bool IsCountryBlocked(string countryCode);
    void LogBlockedAttempt(BlockedAttemptLog log);
    PagedResult<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize);
    void RemoveExpiredBlocks(); // for background cleanup
}