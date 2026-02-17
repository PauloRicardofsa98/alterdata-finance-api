using AlterdataFinanceApi.Application.DTOs.Transaction;
using AlterdataFinanceApi.Domain.Entities;

namespace AlterdataFinanceApi.Application.Mappings;

public static class TransactionMappingExtensions
{
    public static TransactionResponse ToResponse(this Transaction entity)
    {
        return new TransactionResponse(
            Id: entity.Id,
            Description: entity.Description,
            Amount: entity.Amount,
            Date: entity.Date,
            Category: entity.Category,
            Type: entity.Type,
            TypeName: entity.Type.ToString(),
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }

    public static Transaction ToEntity(this CreateTransactionRequest request)
    {
        return new Transaction
        {
            Description = request.Description,
            Amount = request.Amount,
            Date = request.Date,
            Category = request.Category,
            Type = request.Type
        };
    }
}
