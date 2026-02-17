namespace AlterdataFinanceApi.Application.DTOs.Dashboard;

public record MonthlyDataItem(
    int Month,
    string MonthName,
    decimal TotalExpenses,
    decimal TotalRevenues,
    decimal Balance
);
