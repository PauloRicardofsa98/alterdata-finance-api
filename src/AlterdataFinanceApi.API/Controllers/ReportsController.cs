using System.Text;
using AlterdataFinanceApi.API.Helpers;
using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlterdataFinanceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
        var result = await _service.GetReportByPeriodAsync(utcStart, utcEnd, type);
        return Ok(result);
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] TransactionType? type)
    {
        var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
        var report = await _service.GetReportByPeriodAsync(utcStart, utcEnd, type);
        var csv = _service.GenerateCsv(report);
        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray();
        return File(bytes, "text/csv; charset=utf-8", "relatorio.csv");
    }

    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] TransactionType? type)
    {
        var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
        var report = await _service.GetReportByPeriodAsync(utcStart, utcEnd, type);
        var pdf = ReportPdfGenerator.Generate(report);
        return File(pdf, "application/pdf", "relatorio.pdf");
    }
}
