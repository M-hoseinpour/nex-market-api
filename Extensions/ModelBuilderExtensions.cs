using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace market.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Dynamically load all IEntityTypeConfiguration with Reflection
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterEntityTypeConfiguration(
        this ModelBuilder modelBuilder,
        Assembly assembly
    )
    {
        var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods()
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        var types = assembly
            .GetExportedTypes()
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic)
            .ToList();

        foreach (var type in types)
        {
            var iface = type.GetInterfaces()
                .FirstOrDefault(
                    x =>
                        x.IsConstructedGenericType
                        && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                );
            if (iface != null)
            {
                var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(
                    iface.GenericTypeArguments[0]
                );
                applyConcreteMethod.Invoke(
                    obj: modelBuilder,
                    parameters: new object[] { Activator.CreateInstance(type) }
                );
            }
        }
    }

    /// <summary>
    ///     Dynamically register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities<TBaseType>(
        this ModelBuilder modelBuilder,
        params Assembly[] assemblies
    )
    {
        var types = assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(
                c =>
                    c.IsClass
                    && !c.IsAbstract
                    && c.IsPublic
                    && typeof(TBaseType).IsAssignableFrom(c)
            );

        foreach (var type in types)
            modelBuilder.Entity(type);
    }
}