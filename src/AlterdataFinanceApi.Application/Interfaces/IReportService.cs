using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Domain.Enums;

namespace AlterdataFinanceApi.Application.Interfaces;

public interface IReportService
{
    Task<ReportResponse> GetReportByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type);
    string GenerateCsv(ReportResponse report);
}
