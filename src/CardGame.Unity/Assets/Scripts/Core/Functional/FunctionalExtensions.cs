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

        public static TResult ThenReturn<TInput, TResult>(this TInput input, TResult result)
        {
            return result;
        }
    }
}
