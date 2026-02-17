using AlterdataFinanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlterdataFinanceApi.Infrastructure.Data.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.ToTable("administrators");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.HasIndex(a => a.Email)
            .IsUnique();
    }
}
