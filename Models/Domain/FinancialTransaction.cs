using market.Extensions;
using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEF.Models.Enums;

namespace market.Models.Domain;
public class FinancialTransaction : Entity
{
    public decimal Amount { get; set; }
    public FinancialTransactionStatus FinancialTransactionStatus { get; set; }
    public FinancialTransactionType FinancialTransactionType { get; set; }
    public virtual Order? Order { get; set; }
}


public class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.Amount);
        builder.Property(x => x.FinancialTransactionStatus).HasDefaultValue(FinancialTransactionStatus.Pending);
        builder.Property(x => x.FinancialTransactionType);
    }
}