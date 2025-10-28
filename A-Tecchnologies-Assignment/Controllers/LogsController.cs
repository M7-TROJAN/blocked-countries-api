using A_Tecchnologies_Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace A_Tecchnologies_Assignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly IBlockService _blockService;

    public LogsController(IBlockService blockService)
    {
        _blockService = blockService;
    }

    /// <summary>
    /// Retrieves a paginated list of all blocked access attempts.
    /// </summary>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Items per page (default 10).</param>
    /// <response code="200">Returns a paginated list of blocked attempts.</response>
    /// <response code="400">Invalid pagination parameters.</response>
    [HttpGet("blocked-attempts")]
    public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("Page and PageSize must be positive numbers.");

        var result = _blockService.GetBlockedAttempts(page, pageSize);
        return Ok(result);
    }
}