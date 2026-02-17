using AlterdataFinanceApi.Domain.Entities;
using AlterdataFinanceApi.Domain.Enums;
using AlterdataFinanceApi.Domain.Models;

namespace AlterdataFinanceApi.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<IEnumerable<Transaction>> GetByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type);
    Task<IEnumerable<MonthlySummary>> GetMonthlySummaryAsync(int year);
    Task<Transaction> AddAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
    Task DeleteAsync(Guid id);
}
