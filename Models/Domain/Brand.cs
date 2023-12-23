using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = market.Data.Domain.File;

namespace market.Models.Domain;

public class Brand : Entity
{
    public required string Name { get; set; }
    public required int PanelId { get; set; }
    public virtual Panel Panel { get; set; } = null!;

    public Guid? LogoFileId { get; set; }
    public virtual File? LogoFile { get; set; }
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
        
        builder
            .HasOne(x => x.LogoFile)
            .WithOne()
            .HasForeignKey<Brand>(x => x.LogoFileId);
    }
}