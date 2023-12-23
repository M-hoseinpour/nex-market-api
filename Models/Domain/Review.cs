using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Review : Entity
{
    public string? Comment { get; set; }
    public required decimal Rating { get; set; }
    
    public required int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    
    public required int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();

        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.Comment);

        builder
            .HasOne(x => x.Customer)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CustomerId);
        
        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId);
    }
}