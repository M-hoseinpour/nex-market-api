using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Tag : Entity
{
    public required string Name { get; set; }
    public int PanelId { get; set; }
    public virtual required Panel Panel { get; set; }   
    
    public virtual required ICollection<ProductTag> ProductTags { get; set; }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.PanelId).IsRequired();
        
        builder
            .HasOne(x => x.Panel)
            .WithMany()
            .HasForeignKey(x => x.PanelId);
    }
}