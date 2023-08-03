namespace Core.Functional
{
    public static class FunctionalExtensions
    {
        public static T[] ToArray<T>(this T obj) =>
            new T[] { obj };
    }
}
