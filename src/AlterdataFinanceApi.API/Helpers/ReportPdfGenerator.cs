using System.Globalization;
using AlterdataFinanceApi.Application.DTOs.Report;
using AlterdataFinanceApi.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AlterdataFinanceApi.API.Helpers;

public static class ReportPdfGenerator
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public static byte[] Generate(ReportResponse report)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Column(col =>
                {
                    col.Item().Text("Relatório Financeiro")
                        .FontSize(18).Bold().FontColor(Colors.Grey.Darken3);

                    col.Item().Text($"Período: {report.StartDate:dd/MM/yyyy} a {report.EndDate:dd/MM/yyyy}")
                        .FontSize(10).FontColor(Colors.Grey.Medium);

                    col.Item().PaddingBottom(10);
                });

                page.Content().Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            var headerStyle = TextStyle.Default.Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Descrição").Style(headerStyle);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Valor").Style(headerStyle);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Data").Style(headerStyle);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Categoria").Style(headerStyle);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tipo").Style(headerStyle);
                        });

                        foreach (var t in report.Transactions)
                        {
                            var prefix = t.Type == TransactionType.Revenue ? "+" : "-";
                            var valueColor = t.Type == TransactionType.Revenue ? Colors.Green.Darken2 : Colors.Grey.Darken3;

                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(t.Description);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4)
                                .Text($"{prefix} {t.Amount.ToString("C2", PtBr)}").FontColor(valueColor);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(t.Date.ToString("dd/MM/yyyy"));
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(t.Category ?? "—");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4)
                                .Text(t.Type == TransactionType.Revenue ? "Receita" : "Despesa");
                        }
                    });

                    col.Item().PaddingTop(15).Row(row =>
                    {
                        row.RelativeItem().Column(summary =>
                        {
                            summary.Item().Text($"Total Receitas: {report.TotalRevenues.ToString("C2", PtBr)}")
                                .FontSize(10).FontColor(Colors.Green.Darken2);
                            summary.Item().Text($"Total Despesas: {report.TotalExpenses.ToString("C2", PtBr)}")
                                .FontSize(10);
                            summary.Item().Text($"Balanço: {report.Balance.ToString("C2", PtBr)}")
                                .FontSize(11).Bold()
                                .FontColor(report.Balance >= 0 ? Colors.Green.Darken2 : Colors.Red.Darken2);
                        });
                    });
                });

                page.Footer().AlignCenter()
                    .Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }
}
