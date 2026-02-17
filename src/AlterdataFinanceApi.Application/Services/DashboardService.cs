using System.Globalization;
using AlterdataFinanceApi.Application.DTOs.Dashboard;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Domain.Interfaces;

namespace AlterdataFinanceApi.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ITransactionRepository _repository;

    public DashboardService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardResponse> GetDashboardAsync(int year)
    {
        var summaries = await _repository.GetMonthlySummaryAsync(year);
        var summaryDict = summaries.ToDictionary(s => s.Month);

        var monthlyData = Enumerable.Range(1, 12).Select(month =>
        {
            var hasData = summaryDict.TryGetValue(month, out var summary);
            var totalExpenses = hasData ? summary!.TotalExpenses : 0;
            var totalRevenues = hasData ? summary!.TotalRevenues : 0;

            return new MonthlyDataItem(
                Month: month,
                MonthName: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                TotalExpenses: totalExpenses,
                TotalRevenues: totalRevenues,
                Balance: totalRevenues - totalExpenses
            );
        }).ToList();

        return new DashboardResponse(
            Year: year,
            MonthlyData: monthlyData
        );
    }
}
