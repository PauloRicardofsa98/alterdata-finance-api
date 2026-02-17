using AlterdataFinanceApi.Domain.Enums;

namespace AlterdataFinanceApi.Domain.Entities;

public class Transaction : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Category { get; set; }
    public TransactionType Type { get; set; }
}
