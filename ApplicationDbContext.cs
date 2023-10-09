using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace market;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(IEntity).Assembly;
        var configureAssembly = typeof(ApplicationDbContext).Assembly;
        modelBuilder.RegisterAllEntities<IEntity>(assembly);
        modelBuilder.RegisterEntityTypeConfiguration(configureAssembly);
        base.OnModelCreating(modelBuilder);
    }
}
