using AlterdataFinanceApi.Application.DTOs.Dashboard;
using AlterdataFinanceApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlterdataFinanceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(IDashboardService service) : ControllerBase
{
    private readonly IDashboardService _service = service;

    [HttpGet("summary")]
    [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] int? year)
    {
        var result = await _service.GetDashboardAsync(year ?? DateTime.UtcNow.Year);
        return Ok(result);
    }
}
