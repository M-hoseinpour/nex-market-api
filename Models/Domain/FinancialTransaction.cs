using market.Extensions;
using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;
public class FinancialTransaction : Entity
{
    public decimal Amount { get; set; }
    // public FinancialTransactionStatus FinancialTransactionStatus { get; set; }
    // public FinancialTransactionType FinancialTransactionType { get; set; }
    public TransactionFactor TransactionFactor { get; set; }
    public int FinancialDocumentId { get; set; }
    public virtual required FinancialDocument FinancialDocument { get; set; }
    public int FinancialAccountId { get; set; }
    public virtual required FinancialAccount FinancialAccount { get; set; }
}


public class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.Amount);
        builder.Property(x => x.TransactionFactor);
        builder.Property(x => x.FinancialDocumentId).IsRequired();
        builder.Property(x => x.FinancialAccountId).IsRequired();

        builder
            .HasOne(x => x.FinancialAccount)
            .WithMany(x => x.FinancialTransactions)
            .HasForeignKey(x => x.FinancialAccountId);
        
        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.FinancialTransactions)
            .HasForeignKey(x => x.FinancialDocumentId);
    }
}