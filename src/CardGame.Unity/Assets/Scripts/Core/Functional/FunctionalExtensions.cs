using System;

namespace Core.Functional
{
    public static class FunctionalExtensions
    {
        public static T[] ToArray<T>(this T obj) =>
            new T[] { obj };

        public static bool IfTrueDo(this bool value, Action action) 
        {
            if (!value)
                return value;

            action();

            return value;
        }

        public static bool IfFalseDo(this bool value, Action action)
        {
            if (value)
                return value;

            action();

            return value;
        }
    }
}
