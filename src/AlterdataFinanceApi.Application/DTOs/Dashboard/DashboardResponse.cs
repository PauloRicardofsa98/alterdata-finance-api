namespace AlterdataFinanceApi.Application.DTOs.Dashboard;

public record DashboardResponse(
    int Year,
    List<MonthlyDataItem> MonthlyData
);
