using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Data.Domain;

public class Role : IEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Id).HasMaxLength(120);

        builder.HasData(new List<Role>
        {
            new()
            {
                Id = 1,
                Slug = "customer",
                Title = "کاربر"
            },
            new()
            {
                Id = 2,
                Slug = "staff",
                Title = "کارمند"
            },
            new()
            {
                Id = 3,
                Slug = "manager",
                Title = "مدیر"
            }
        });
    }
}