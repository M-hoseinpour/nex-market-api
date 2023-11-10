using Auctioneer.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using market.Models.Common;

namespace market.Data.Domain;

public class File : Entity<Guid?>, IWithSoftDelete, IWithTimestamps
{
    public string S3ObjectKey { get; set; } = null!;
    public short CategoryId { get; set; }
    public FileCategory Category { get; set; } = null!;
}

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasTimestamps();
        builder.HasSoftDelete();

        builder
            .HasKey(e => e.Id)
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        builder.Property(x => x.CreateMoment).IsRequired();
        builder.Property(x => x.UpdateMoment);
        builder.Property(x => x.DeleteMoment);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(x => x.S3ObjectKey).IsUnique();

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.CategoryId);
    }
}