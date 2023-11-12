using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class CartItem : Entity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasSoftDelete();
        builder.HasTimestamps();

        builder.Property(x => x.Quantity).HasDefaultValue(1);
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();

        builder
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId);
        
        builder
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);
    }
}