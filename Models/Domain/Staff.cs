using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Staff : Entity
{
    public int NationalId { get; set; }
    public int ApproverId { get; set; }
    public int UserId { get; set; }
    public virtual required User User { get; set; }
    public int? PanelId { get; set; }
    public virtual Panel? Panel { get; set; }

}

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {

        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.NationalId);
        builder.Property(x => x.ApproverId);

        builder
         .HasOne(x => x.User)
         .WithOne(x => x.Staff)
         .HasForeignKey<Staff>(x => x.UserId);

        builder
         .HasOne(x => x.Panel)
         .WithMany(x => x.Staffs)
         .HasForeignKey(x => x.PanelId);
    }
}