using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Application.Mappings;
using AlterdataFinanceApi.Domain.Enums;
using AlterdataFinanceApi.Domain.Interfaces;

namespace AlterdataFinanceApi.Application.Services;

public class ReportService : IReportService
{
    private readonly ITransactionRepository _repository;

    public ReportService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReportResponse> GetReportByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type)
    {
        var transactions = await _repository.GetByPeriodAsync(startDate, endDate, type);
        var transactionList = transactions.Select(t => t.ToResponse()).ToList();

        var totalExpenses = transactionList
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        var totalRevenues = transactionList
            .Where(t => t.Type == TransactionType.Revenue)
            .Sum(t => t.Amount);

        return new ReportResponse(
            Transactions: transactionList,
            TotalExpenses: totalExpenses,
            TotalRevenues: totalRevenues,
            Balance: totalRevenues - totalExpenses,
            StartDate: startDate,
            EndDate: endDate
        );
    }
}
