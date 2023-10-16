using Auctioneer.Data.Extensions;
using market.Data.Domain;
using market.Models.Common;
using market.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class User : Entity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? MobileNumber { get; set; }
    public UserState UserState { get; set; } = UserState.Pending;
    public string? AvatarLogo { get; set; }
    public string? EmailVerifiedAt { get; set; }
    public string? Setting { get; set; }
    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual Staff? Staff { get; set; }
    public virtual Manager? Manager { get; set; }

}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.FirstName);
        builder.Property(x => x.LastName);
        builder.Property(x => x.RoleId).HasDefaultValue(Roles.Customer.Id);
        builder.Property(x => x.UserState).HasDefaultValue(UserState.Pending);
        builder.Property(x => x.AvatarLogo);
        builder.Property(x => x.Setting);
        builder.Property(x => x.EmailVerifiedAt);
        builder.Property(x => x.MobileNumber);

        builder
           .HasOne(x => x.Role)
           .WithMany()
           .HasForeignKey(x => x.RoleId);
    }
}