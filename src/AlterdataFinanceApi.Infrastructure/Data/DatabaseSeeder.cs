using AlterdataFinanceApi.Domain.Entities;
using AlterdataFinanceApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AlterdataFinanceApi.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Administrators.AnyAsync())
            return;

        var admin = new Administrator(
            "Administrador",
            "admin@alterdata.com.br",
            BCrypt.Net.BCrypt.HashPassword("Admin@123")
        );
        context.Administrators.Add(admin);

        var random = new Random(42);
        var transactions = GenerateTransactions(random);
        context.Transactions.AddRange(transactions);

        await context.SaveChangesAsync();
    }

    private static List<Transaction> GenerateTransactions(Random random)
    {
        var expenseCategories = new Dictionary<string, string[]>
        {
            ["Alimentação"] = ["Supermercado", "Restaurante", "Padaria", "Delivery iFood", "Feira", "Lanche"],
            ["Moradia"] = ["Aluguel", "Condomínio", "Energia elétrica", "Água", "Gás", "Internet"],
            ["Transporte"] = ["Combustível", "Uber", "Estacionamento", "Manutenção veículo", "IPVA", "Pedágio"],
            ["Saúde"] = ["Farmácia", "Consulta médica", "Plano de saúde", "Exames", "Dentista"],
            ["Educação"] = ["Curso online", "Livros", "Mensalidade faculdade", "Material escolar"],
            ["Lazer"] = ["Cinema", "Streaming Netflix", "Spotify", "Viagem", "Restaurante bar", "Academia"],
            ["Vestuário"] = ["Roupas", "Calçados", "Acessórios"],
            ["Serviços"] = ["Celular", "Assinatura software", "Contador", "Seguro auto"],
        };

        var revenueCategories = new Dictionary<string, string[]>
        {
            ["Salário"] = ["Salário mensal"],
            ["Freelance"] = ["Projeto freelance", "Consultoria", "Desenvolvimento web"],
            ["Investimentos"] = ["Dividendos", "Rendimento CDB", "Rendimento poupança"],
            ["Outros"] = ["Venda de item usado", "Cashback", "Reembolso"],
        };

        var transactions = new List<Transaction>();
        var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc);

        for (var date = startDate; date <= endDate; date = date.AddMonths(1))
        {
            // Monthly salary
            transactions.Add(new Transaction(
                "Salário mensal",
                RandomDecimal(random, 5000, 8000),
                new DateTime(date.Year, date.Month, 5, 0, 0, 0, DateTimeKind.Utc),
                "Salário",
                TransactionType.Revenue
            ));

            // Occasional freelance (40% chance)
            if (random.NextDouble() < 0.4)
            {
                var freelanceDescs = revenueCategories["Freelance"];
                transactions.Add(new Transaction(
                    freelanceDescs[random.Next(freelanceDescs.Length)],
                    RandomDecimal(random, 500, 3000),
                    RandomDateInMonth(random, date),
                    "Freelance",
                    TransactionType.Revenue
                ));
            }

            // Investment returns (30% chance)
            if (random.NextDouble() < 0.3)
            {
                var investDescs = revenueCategories["Investimentos"];
                transactions.Add(new Transaction(
                    investDescs[random.Next(investDescs.Length)],
                    RandomDecimal(random, 100, 1500),
                    RandomDateInMonth(random, date),
                    "Investimentos",
                    TransactionType.Revenue
                ));
            }

            // Fixed monthly expenses
            transactions.Add(new Transaction("Aluguel", RandomDecimal(random, 1200, 1800), new DateTime(date.Year, date.Month, 10, 0, 0, 0, DateTimeKind.Utc), "Moradia", TransactionType.Expense));
            transactions.Add(new Transaction("Condomínio", RandomDecimal(random, 300, 500), new DateTime(date.Year, date.Month, 10, 0, 0, 0, DateTimeKind.Utc), "Moradia", TransactionType.Expense));
            transactions.Add(new Transaction("Energia elétrica", RandomDecimal(random, 80, 250), RandomDateInMonth(random, date), "Moradia", TransactionType.Expense));
            transactions.Add(new Transaction("Internet", 119.90m, new DateTime(date.Year, date.Month, 15, 0, 0, 0, DateTimeKind.Utc), "Moradia", TransactionType.Expense));
            transactions.Add(new Transaction("Plano de saúde", RandomDecimal(random, 350, 500), new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc), "Saúde", TransactionType.Expense));

            // Variable expenses (8-12 per month)
            var variableCount = random.Next(8, 13);
            var categories = expenseCategories.Keys.ToArray();

            for (var i = 0; i < variableCount; i++)
            {
                var category = categories[random.Next(categories.Length)];
                var descriptions = expenseCategories[category];
                var description = descriptions[random.Next(descriptions.Length)];

                var (minAmount, maxAmount) = category switch
                {
                    "Alimentação" => (15m, 400m),
                    "Transporte" => (20m, 500m),
                    "Saúde" => (30m, 600m),
                    "Educação" => (50m, 800m),
                    "Lazer" => (15m, 300m),
                    "Vestuário" => (50m, 500m),
                    "Serviços" => (30m, 300m),
                    _ => (20m, 400m),
                };

                transactions.Add(new Transaction(
                    description,
                    RandomDecimal(random, minAmount, maxAmount),
                    RandomDateInMonth(random, date),
                    category,
                    TransactionType.Expense
                ));
            }
        }

        return transactions;
    }

    private static decimal RandomDecimal(Random random, decimal min, decimal max)
    {
        var range = (double)(max - min);
        return Math.Round(min + (decimal)(random.NextDouble() * range), 2);
    }

    private static DateTime RandomDateInMonth(Random random, DateTime monthStart)
    {
        var daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        var day = random.Next(1, daysInMonth + 1);
        return new DateTime(monthStart.Year, monthStart.Month, day, 0, 0, 0, DateTimeKind.Utc);
    }
}
