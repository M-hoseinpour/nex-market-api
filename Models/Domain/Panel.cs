using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Panel : Entity
{
    public required string Name { get; set; }
    public int ManagerId { get; set; }
    public virtual Manager? Manager { get; set; }
    public virtual ICollection<Staff>? Staffs { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
}

public class PanelConfiguration : IEntityTypeConfiguration<Panel>
{
    public void Configure(EntityTypeBuilder<Panel> builder)
    {
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name);

        builder
        .HasOne(x => x.Manager)
        .WithOne(x => x.Panel)
        .HasForeignKey<Panel>(x => x.ManagerId);

    }
}