namespace AlterdataFinanceApi.Domain.Models;

public class MonthlySummary
{
    public int Month { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalRevenues { get; set; }
}
