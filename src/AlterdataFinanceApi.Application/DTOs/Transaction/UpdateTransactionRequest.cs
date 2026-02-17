using AlterdataFinanceApi.Domain.Enums;

namespace AlterdataFinanceApi.Application.DTOs.Transaction;

public record UpdateTransactionRequest(
    string Description,
    decimal Amount,
    DateTime Date,
    string? Category,
    TransactionType Type
);
