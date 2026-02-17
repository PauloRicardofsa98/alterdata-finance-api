using AlterdataFinanceApi.Domain.Enums;

namespace AlterdataFinanceApi.Domain.Entities;

public class Transaction : BaseEntity
{
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string? Category { get; private set; }
    public TransactionType Type { get; private set; }

    private Transaction() { }

    public Transaction(string description, decimal amount, DateTime date, string? category, TransactionType type)
    {
        Description = description;
        Amount = amount;
        Date = date;
        Category = category;
        Type = type;
    }

    public void Update(string description, decimal amount, DateTime date, string? category, TransactionType type)
    {
        Description = description;
        Amount = amount;
        Date = date;
        Category = category;
        Type = type;
    }
}
