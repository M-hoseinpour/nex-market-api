using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class FinancialDocument : Entity
{
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual Order Order { get; set; }
}

public class FinancialDocumentConfiguration : IEntityTypeConfiguration<FinancialDocument>
{
    public void Configure(EntityTypeBuilder<FinancialDocument> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();
    }
}