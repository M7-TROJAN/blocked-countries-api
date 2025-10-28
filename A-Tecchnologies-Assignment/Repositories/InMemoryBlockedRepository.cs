using A_Tecchnologies_Assignment.Models;
using System.Collections.Concurrent;

namespace A_Tecchnologies_Assignment.Repositories;

public class InMemoryBlockedRepository : IBlockedRepository
{
    private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();
    private readonly ConcurrentBag<BlockedAttemptLog> _blockedAttempts = new();
    //private readonly List<BlockedAttemptLog> _blockedAttempts = new();
    //private readonly object _logLock = new(); // thread-safety for logs

    public bool AddBlockedCountry(BlockedCountry country)
    {
        return _blockedCountries.TryAdd(country.CountryCode.ToUpper(), country);
    }

    public bool AddOrUpdateBlockedCountry(BlockedCountry country)
    {
        _blockedCountries.AddOrUpdate(country.CountryCode.ToUpper(), country, (key, oldValue) => country);
        return true;
    }

    public bool RemoveBlockedCountry(string countryCode)
    {
        return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
    }

    public IEnumerable<BlockedCountry> GetBlockedCountries()
    {
        return _blockedCountries.Values.ToList();
    }

    public BlockedCountry? GetCountryByCode(string countryCode)
    {
        _blockedCountries.TryGetValue(countryCode.ToUpper(), out var country);
        return country;
    }

    public void AddBlockedAttempt(BlockedAttemptLog log)
    {
        _blockedAttempts.Add(log);
    }

    public IEnumerable<BlockedAttemptLog> GetBlockedAttempts()
    {
        return _blockedAttempts.ToList();
    }

    //public void AddBlockedAttempt(BlockedAttemptLog log)
    //{
    //    lock (_logLock)
    //    {
    //        _blockedAttempts.Add(log);
    //    }
    //}

    //public IEnumerable<BlockedAttemptLog> GetBlockedAttempts()
    //{
    //    lock (_logLock)
    //    {
    //        return _blockedAttempts.ToList();
    //    }
    //}
}