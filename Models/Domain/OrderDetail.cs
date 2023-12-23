using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class OrderDetail : Entity
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    
    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;
}

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasSoftDelete();
        builder.HasTimestamps();

        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Quantity).HasDefaultValue(1);
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.OrderId).IsRequired();
        
        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.OrderId);
        
        builder
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);
    }
}