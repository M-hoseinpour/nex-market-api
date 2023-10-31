using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Product : Entity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal? Rating  { get; set; }
    public int PanelId { get; set; }
    public virtual required Panel Panel { get; set; }
    public virtual required ICollection<ProductTag> ProductTags { get; set; }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.Detail);
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Rating);
        builder.Property(x => x.PanelId).IsRequired();

        builder
            .HasOne(x => x.Panel)
            .WithMany()
            .HasForeignKey(x => x.PanelId);
    }
}