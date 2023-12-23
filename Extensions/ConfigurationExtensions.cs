using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void HasUuid<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IEntity, IWithUuid
        {
            builder.Property(x => x.Uuid).IsRequired();
        }

        public static void HasSoftDelete<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IEntity, IWithSoftDelete
        {
            builder.Property(x => x.DeleteMoment);
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }

        public static void HasTimestamps<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IEntity, IWithTimestamps
        {
            builder.Property(x => x.CreateMoment).IsRequired();
            builder.Property(x => x.UpdateMoment);
        }
    }
}