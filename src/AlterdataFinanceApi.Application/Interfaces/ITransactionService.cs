using AlterdataFinanceApi.Application.DTOs.Transaction;

namespace AlterdataFinanceApi.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);
    Task<TransactionResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<TransactionResponse>> GetAllAsync();
    Task<TransactionResponse> UpdateAsync(Guid id, UpdateTransactionRequest request);
    Task DeleteAsync(Guid id);
}
