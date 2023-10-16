using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Manager : Entity
{
    public string? NationalId { get; set; }
    public string? DocumentUrl { get; set; }
    public int UserId { get; set; }
    public virtual required User User { get; set; }

}

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DocumentUrl);
        builder.Property(x => x.NationalId);

        builder
         .HasOne(x => x.User)
         .WithOne(x => x.Manager)
         .HasForeignKey<Manager>(x => x.UserId);
    }
}