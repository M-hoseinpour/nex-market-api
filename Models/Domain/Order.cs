using Auctioneer.Data.Extensions;
using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Order : Entity
{
    public OrderStatus Status { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public int AddressId { get; set; }
    public virtual Address Address { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasSoftDelete();
        builder.HasTimestamps();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.AddressId).IsRequired();
        builder.Property(x => x.Status).HasDefaultValue(OrderStatus.Pending);
        
        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        
        builder
            .HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);
    }
}