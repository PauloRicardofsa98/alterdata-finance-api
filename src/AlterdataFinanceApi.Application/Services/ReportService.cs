using System.Globalization;
using System.Text;
using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Application.Mappings;
using AlterdataFinanceApi.Domain.Enums;
using AlterdataFinanceApi.Domain.Interfaces;

namespace AlterdataFinanceApi.Application.Services;

public class ReportService : IReportService
{
    private readonly ITransactionQueryRepository _queryRepository;

    public ReportService(ITransactionQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<ReportResponse> GetReportByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type)
    {
        var transactions = await _queryRepository.GetByPeriodAsync(startDate, endDate, type);
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

    public string GenerateCsv(ReportResponse report)
    {
        var culture = new CultureInfo("pt-BR");
        var sb = new StringBuilder();
        sb.AppendLine("Descrição;Valor;Data;Categoria;Tipo");

        foreach (var t in report.Transactions)
        {
            var valor = t.Amount.ToString("N2", culture);
            var data = t.Date.ToString("dd/MM/yyyy");
            var categoria = t.Category ?? "";
            var tipo = t.Type == TransactionType.Revenue ? "Receita" : "Despesa";
            sb.AppendLine($"{t.Description};{valor};{data};{categoria};{tipo}");
        }

        sb.AppendLine();
        sb.AppendLine($"Total Receitas;{report.TotalRevenues.ToString("N2", culture)};;;");
        sb.AppendLine($"Total Despesas;{report.TotalExpenses.ToString("N2", culture)};;;");
        sb.AppendLine($"Balanço;{report.Balance.ToString("N2", culture)};;;");

        return sb.ToString();
    }
}
