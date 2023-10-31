using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Category : Entity
{
    public required string Name { get; set; }
    public int PanelId { get; set; }
    public virtual required Panel Panel { get; set; }
    public int? ParentCategoryId { get; set; }
    public virtual required Category ParentCategory { get; set; }
    public virtual required ICollection<Category> ChildCategories { get; set; }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
        
        builder
            .HasOne(x => x.ParentCategory)
            .WithMany(x => x.ChildCategories)
            .HasForeignKey(x => x.ParentCategoryId);
    }
}