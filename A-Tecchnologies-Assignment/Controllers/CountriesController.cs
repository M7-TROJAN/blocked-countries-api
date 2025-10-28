using A_Tecchnologies_Assignment.DTOs;
using A_Tecchnologies_Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace A_Tecchnologies_Assignment.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly IBlockService _blockService;

    public CountriesController(IBlockService blockService)
    {
        _blockService = blockService;
    }

    /// <summary>
    /// Permanently blocks a country from accessing the system.
    /// </summary>
    /// <param name="request">Country code and name to block.</param>
    /// <response code="200">Country blocked successfully.</response>
    /// <response code="400">Invalid country data provided.</response>
    /// <response code="409">Country already blocked.</response>
    [HttpPost("block")]
    public IActionResult BlockCountry([FromBody] BlockCountryRequest request)
    {
        var success = _blockService.BlockCountry(request);
        if (!success)
            return Conflict("Country already blocked.");

        return Ok(new { message = $"Country '{request.CountryName}' blocked successfully." });
    }

    /// <summary>
    /// Unblocks a blocked country.
    /// </summary>
    /// <param name="countryCode">ISO country code.</param>
    /// <response code="200">Country unblocked successfully.</response>
    /// <response code="404">Country not found or not blocked.</response>
    [HttpDelete("block/{countryCode}")]
    // but i think [HttpDelete("unblock/{countryCode}")] is better for naming convention
    public IActionResult UnblockCountry(string countryCode)
    {
        var removed = _blockService.UnblockCountry(countryCode);
        if (!removed)
            return NotFound("Country not found or not blocked.");

        return Ok(new { message = $"Country '{countryCode}' unblocked successfully." });
    }

    /// <summary>
    /// Retrieves a paginated list of all blocked countries.
    /// </summary>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Items per page (default 10).</param>
    /// <param name="search">Optional search term for country code or name.</param>
    /// <response code="200">Returns the paginated list of blocked countries.</response>
    /// <response code="400">Invalid pagination parameters.</response>
    [HttpGet("blocked")]
    public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("Page and PageSize must be positive numbers.");

        var result = _blockService.GetBlockedCountries(page, pageSize, search);
        return Ok(result);
    }

    /// <summary>
    /// Temporarily blocks a country for a specific duration (1–1440 minutes).
    /// </summary>
    /// <param name="request">Country data and duration in minutes.</param>
    /// <response code="200">Country blocked temporarily.</response>
    /// <response code="400">Invalid input or duration outside allowed range.</response>
    /// <response code="409">Country already blocked.</response>
    [HttpPost("temporal-block")]
    public IActionResult TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
    {
        try
        {
            var success = _blockService.TemporarilyBlockCountry(request);
            if (!success)
                return Conflict("Country already blocked.");

            return Ok(new
            {
                message = $"Country '{request.CountryName}' blocked temporarily for {request.DurationMinutes} minutes."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}