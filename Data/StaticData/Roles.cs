using market.Data.Domain;

public static class Roles
{
    public static Role Customer { get; } =
        new()
        {
            Id = 1,
            Slug = "customer",
            Title = "کاربر"
        };

    public static Role Staff { get; } =
        new()
        {
            Id = 2,
            Slug = "staff",
            Title = "کارمند"
        };

    public static Role Manager { get; } =
        new()
        {
            Id = 3,
            Slug = "manager",
            Title = "مدیر"
        };
}