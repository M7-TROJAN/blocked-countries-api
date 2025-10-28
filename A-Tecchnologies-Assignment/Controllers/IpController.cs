using A_Tecchnologies_Assignment.Models;
using A_Tecchnologies_Assignment.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IpController : ControllerBase
{
    private readonly IGeoLocationService _geoService;
    private readonly IBlockService _blockService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IpController(IGeoLocationService geoService, IBlockService blockService, IHttpContextAccessor httpContextAccessor)
    {
        _geoService = geoService;
        _blockService = blockService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Looks up geolocation information for a given IP address.
    /// </summary>
    /// <param name="ipAddress">Optional IP address to check. If omitted, the client's IP is used.</param>
    /// <response code="200">Returns geolocation data.</response>
    /// <response code="400">Invalid or missing IP address.</response>
    /// <response code="502">Failed to fetch data from the external Geo API.</response>
    [HttpGet("lookup")]
    public async Task<IActionResult> LookupIp([FromQuery] string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        if (string.IsNullOrWhiteSpace(ipAddress))
            return BadRequest("Unable to determine IP address.");

        var result = await _geoService.GetGeoInfoAsync(ipAddress);
        if (result == null)
            return StatusCode(502, "Failed to retrieve IP information from Geo API.");

        return Ok(result);
    }

    /// <summary>
    /// Checks whether the caller's country is currently blocked.
    /// </summary>
    /// <response code="200">Access allowed for the detected country.</response>
    /// <response code="403">Access denied due to country block.</response>
    /// <response code="400">Could not determine the client's IP address.</response>
    /// <response code="502">Failed to fetch data from the external Geo API.</response>
    [HttpGet("check-block")]
    public async Task<IActionResult> CheckIfBlocked()
    {
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        if (string.IsNullOrWhiteSpace(ip))
            return BadRequest("Unable to determine client IP.");

        var geoInfo = await _geoService.GetGeoInfoAsync(ip);
        if (geoInfo == null)
            return StatusCode(502, "Failed to retrieve IP information from Geo API.");

        var isBlocked = _blockService.IsCountryBlocked(geoInfo.CountryCode);

        if (isBlocked)
        {
            // Log it
            var log = new BlockedAttemptLog
            {
                IpAddress = ip,
                CountryCode = geoInfo.CountryCode,
                IsBlocked = true,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                Timestamp = DateTime.UtcNow
            };

            _blockService.LogBlockedAttempt(log);

            return StatusCode(403, new { message = $"Access denied for country: {geoInfo.CountryName}" });
        }

        return Ok(new { message = $"Access allowed for {geoInfo.CountryName}" });
    }
}