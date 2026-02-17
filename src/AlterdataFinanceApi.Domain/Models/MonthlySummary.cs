namespace AlterdataFinanceApi.Domain.Models;

public record MonthlySummary
{
    public int Month { get; init; }
    public decimal TotalExpenses { get; init; }
    public decimal TotalRevenues { get; init; }
}
