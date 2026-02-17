using AlterdataFinanceApi.Domain.Enums;

namespace AlterdataFinanceApi.Application.DTOs.Transaction;

public record TransactionResponse(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    string? Category,
    TransactionType Type,
    string TypeName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
