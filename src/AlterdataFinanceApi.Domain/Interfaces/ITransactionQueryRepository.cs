using AlterdataFinanceApi.Domain.Entities;
using AlterdataFinanceApi.Domain.Enums;
using AlterdataFinanceApi.Domain.Models;

namespace AlterdataFinanceApi.Domain.Interfaces;

public interface ITransactionQueryRepository
{
    Task<IEnumerable<Transaction>> GetByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type);
    Task<IEnumerable<MonthlySummary>> GetMonthlySummaryAsync(int year);
}
