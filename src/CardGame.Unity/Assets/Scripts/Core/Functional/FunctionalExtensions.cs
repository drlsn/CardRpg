using System;

namespace Core.Functional
{
    public static class FunctionalExtensions
    {
        public static T[] ToArray<T>(this T obj) =>
            new T[] { obj };

        public static T IfNull<T>(this T value, Action action)
            where T : class
        {
            if (value is not null)
                return value;

            action();

            return value;
        }

        public static T IfNotNull<T>(this T value, Action<T> action)
            where T : class
        {
            if (value is null)
                return value;

            action(value);

            return value;
        }

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

        public static TInput[] And<TInput>(this TInput left, TInput right) =>
            new[] { left, right };

        public static Action Then(this Action action1, Action action2) => () => 
        {
            action1();
            action2();
        };

        public static T Then<T>(this T input, Action action) 
        {
            action?.Invoke();
            return input;
        }

        public static T Then<T>(this T input, Action<T> action)
        {
            action?.Invoke(input);
            return input;
        }

        public static TResult ThenReturn<TInput, TResult>(this TInput input, TResult result)
        {
            return result;
        }
    }
}
