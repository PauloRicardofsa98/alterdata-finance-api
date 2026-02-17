using AlterdataFinanceApi.Application.DTOs.Transaction;

namespace AlterdataFinanceApi.Application.DTOs.Report;

public record ReportResponse(
    List<TransactionResponse> Transactions,
    decimal TotalExpenses,
    decimal TotalRevenues,
    decimal Balance,
    DateTime StartDate,
    DateTime EndDate
);
