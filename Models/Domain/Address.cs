using Auctioneer.Data.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Address : Entity
{
    public required string Location { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public int CustomerId { get; set; }
    public virtual required Customer Customer { get; set; }

}


public class ExpertConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {

        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Latitude);
        builder.Property(x => x.Location);
        builder.Property(x => x.Longitude);

    }
}