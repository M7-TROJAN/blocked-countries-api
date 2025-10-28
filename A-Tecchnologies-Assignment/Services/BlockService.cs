using A_Tecchnologies_Assignment.DTOs;
using A_Tecchnologies_Assignment.Models;
using A_Tecchnologies_Assignment.Repositories;

namespace A_Tecchnologies_Assignment.Services;

public class BlockService : IBlockService
{
    private readonly IBlockedRepository _repository;

    public BlockService(IBlockedRepository repository)
    {
        _repository = repository;
    }

    public bool BlockCountry(BlockCountryRequest request)
    {
        var country = new BlockedCountry
        {
            CountryCode = request.CountryCode.ToUpper(),
            CountryName = request.CountryName,
            ExpirationTime = null // permanent
        };

        return _repository.AddBlockedCountry(country);
    }

    public bool UnblockCountry(string countryCode)
    {
        return _repository.RemoveBlockedCountry(countryCode);
    }

    public PagedResult<BlockedCountry> GetBlockedCountries(int page, int pageSize, string? search = null)
    {
        var all = _repository.GetBlockedCountries();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            all = all.Where(c =>
                c.CountryCode.ToLower().Contains(search) ||
                c.CountryName.ToLower().Contains(search));
        }

        var total = all.Count();
        var items = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<BlockedCountry>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public bool TemporarilyBlockCountry(TemporalBlockRequest request)
    {
        if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
            throw new ArgumentException("Duration must be between 1 and 1440 minutes.");

        var existing = _repository.GetCountryByCode(request.CountryCode);
        if (existing != null && existing.ExpirationTime == null)
            return false; // already permanently blocked

        var country = new BlockedCountry
        {
            CountryCode = request.CountryCode.ToUpper(),
            CountryName = request.CountryName,
            ExpirationTime = DateTime.UtcNow.AddMinutes(request.DurationMinutes)
        };

        return _repository.AddOrUpdateBlockedCountry(country);
    }

    public bool IsCountryBlocked(string countryCode)
    {
        var country = _repository.GetCountryByCode(countryCode);
        if (country == null)
            return false;

        // Check if temporal block expired
        if (country.ExpirationTime != null && country.ExpirationTime < DateTime.UtcNow)
        {
            //_repository.RemoveBlockedCountry(country.CountryCode); // the background job will handle this 
            return false;
        }

        return true;
    }

    public void LogBlockedAttempt(BlockedAttemptLog log)
    {
        _repository.AddBlockedAttempt(log);
    }

    public PagedResult<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize)
    {
        var logs = _repository.GetBlockedAttempts()
            .OrderByDescending(l => l.Timestamp);

        var total = logs.Count();
        var items = logs
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<BlockedAttemptLog>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public void RemoveExpiredBlocks()
    {
        var countries = _repository.GetBlockedCountries();
        foreach (var country in countries)
        {
            if (country.ExpirationTime != null && country.ExpirationTime < DateTime.UtcNow)
            {
                _repository.RemoveBlockedCountry(country.CountryCode);
            }
        }
    }
}