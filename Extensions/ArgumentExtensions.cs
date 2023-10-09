namespace market.Extensions
{
    public static class ArgumentExtensions
    {
        public static void NotNull<T>(this T obj, string name, string message = null)
            where T : class
        {
            if (obj is null)
                throw new ArgumentNullException($"{name} : {typeof(T)}", message);
        }
    }
}