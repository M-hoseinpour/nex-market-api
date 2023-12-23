using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = market.Data.Domain.File;

namespace market.Models.Domain;

public class Banner : Entity
{
    public required string Title { get; set; }
    public Guid FileId { get; set; }
    public virtual required File File { get; set; }
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

        builder
            .HasOne(x => x.File)
            .WithOne()
            .HasForeignKey<Banner>(x => x.FileId);
    }
}