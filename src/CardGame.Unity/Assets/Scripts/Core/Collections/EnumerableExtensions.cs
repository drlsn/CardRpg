using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Collections
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable is null || enumerable.Count() <= 0;

        public static void ForEach(this int index, Action action)
        {
            index = System.Math.Abs(index);
            for (int i = 0; i < index; i++)
                action();
        }

        public static void ForEach(this int index, Action<int> action)
        {
            index = System.Math.Abs(index);
            for (int i = 0; i < index; i++)
                action(i);
        }

        public static void ForEachEnd<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> itemAction)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item, i);

                i++;
            }

            return enumerable;
        }

        public static T AggregateOrDefault<T>(this IEnumerable<T> source, Func<T, T, T> aggregator)
        {
            if (source.IsNullOrEmpty())
                return default;

            if (source.Count() == 1)
                return source.First();

            return source.Aggregate(aggregator);
        }

        public static T AggregateOrEmpty<T>(this IEnumerable<T> source, Func<T, T, T> aggregator, T empty)
        {
            if (source.IsNullOrEmpty())
                return empty;

            return source.AggregateOrDefault(aggregator);
        }
    }
}
