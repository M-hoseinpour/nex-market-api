using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = market.Data.Domain.File;

namespace market.Models.Domain;

public class Banner : Entity
{
    public required string Title { get; set; }
    public string? Detail { get; set; }
    public Guid FileId { get; set; }
    public virtual File File { get; set; } = null!;
    public int PanelId { get; set; }
    public virtual Panel Panel { get; set; } = null!;
}

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasTimestamps();
        builder.HasSoftDelete();

        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.FileId).IsRequired();
        builder.Property(x => x.PanelId).IsRequired();
        builder.Property(x => x.Detail);


        builder
            .HasOne(x => x.File)
            .WithOne()
            .HasForeignKey<Banner>(x => x.FileId);

        builder
            .HasOne(x => x.Panel)
            .WithMany()
            .HasForeignKey(x => x.PanelId);
    }
}