using AlterdataFinanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlterdataFinanceApi.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.Category)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(t => t.Date)
            .HasDatabaseName("IX_transactions_date");

        builder.HasIndex(t => t.Type)
            .HasDatabaseName("IX_transactions_type");
    }
}
