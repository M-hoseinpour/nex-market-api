using market.Extensions;
using market.Models.Common;
using market.Models.Enum;
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
    public ProductStatus Status { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int PanelId { get; set; }
    public virtual required Panel Panel { get; set; }
    public int BrandId { get; set; }
    public virtual required Brand Brand { get; set; }
    public int CategoryId { get; set; }
    public virtual required Category Category { get; set; }
    public virtual required ICollection<ProductTag> ProductTags { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<ProductImage>? Images { get; set; }
    
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
        builder.Property(x => x.Status).HasDefaultValue(ProductStatus.Ordered);
        builder.Property(x => x.PanelId).IsRequired();
        builder.Property(x => x.DiscountPrice);

        builder
            .HasOne(x => x.Panel)
            .WithMany()
            .HasForeignKey(x => x.PanelId);

        builder
            .HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}