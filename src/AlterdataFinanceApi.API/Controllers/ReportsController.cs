using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AlterdataFinanceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService service) : ControllerBase
{
    private readonly IReportService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ReportResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] TransactionType? type)
    {
        var result = await _service.GetReportByPeriodAsync(startDate, endDate, type);
        return Ok(result);
    }
}
