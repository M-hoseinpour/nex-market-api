using market.Extensions;
using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class BankTransaction : Entity
{
    public required string ReferenceCode { get; set; }
    public decimal Amount { get; set; }
    public BanTransactionStatus State { get; set; }
    public int CustomerId { get; set; }
    public required string Session { get; set; }

    public virtual required Customer Customer { get; set; }
}

public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
{
    public void Configure(EntityTypeBuilder<BankTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.ReferenceCode).IsRequired();
        builder.Property(x => x.State).IsRequired().HasDefaultValue(BanTransactionStatus.Pending);
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Session).IsRequired();

        builder
            .HasOne(x => x.Customer)
            .WithMany(x => x.BankTransactions)
            .HasForeignKey(x => x.CustomerId);
    }
}