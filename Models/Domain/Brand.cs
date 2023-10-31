using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Brand : Entity
{
    public required string Name { get; set; }
    public int PanelId { get; set; }
    public virtual required Panel Panel { get; set; }
    
    //todo after adding file system we should add logo file Id here
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
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