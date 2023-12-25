using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class FinancialAccount : Entity
{
    public int? PanelId { get; set; }
    public int? CustomerId { get; set; }
    public virtual Panel? Panel { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
}

public class FinancialAccountConfiguration : IEntityTypeConfiguration<FinancialAccount>
{
    public void Configure(EntityTypeBuilder<FinancialAccount> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.PanelId);
        builder.Property(x => x.CustomerId);
        
        builder
            .HasOne(x => x.Panel)
            .WithOne(x => x.FinancialAccount)
            .HasForeignKey<FinancialAccount>(x => x.PanelId);
        
        builder
            .HasOne(x => x.Customer)
            .WithOne(x => x.FinancialAccount)
            .HasForeignKey<FinancialAccount>(x => x.CustomerId);
    }
}