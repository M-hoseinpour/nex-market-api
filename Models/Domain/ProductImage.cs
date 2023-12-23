using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = market.Data.Domain.File;

namespace market.Models.Domain;

public class ProductImage : IEntity
{
    public int Id { get; set; }
    public ProductImageType Type { get; set; }
    public required Guid FileId { get; set; }
    public virtual File File { get; set; } = null!;
    public required int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
}

public class ProductGalleryConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.FileId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();

        builder.HasOne(x => x.File).WithOne().HasForeignKey<ProductImage>(x => x.FileId);

        builder.HasOne(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductId);
    }
}