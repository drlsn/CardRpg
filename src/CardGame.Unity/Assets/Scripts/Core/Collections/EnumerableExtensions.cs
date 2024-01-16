using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Collections
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable is null || enumerable.Count() <= 0;

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) =>
            source.Where(x => x is not null);

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
            if (enumerable == null)
                return;

            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }
        }

        public static IEnumerable<T> ForEachReversed<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            var list = enumerable.ToList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (item != null)
                    itemAction(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            if (enumerable == null)
                return null;

            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> itemAction)
        {
            if (enumerable == null)
                return null;

            int i = 0;
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item, i);

                i++;
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int, int> itemAction)
        {
            if (enumerable == null)
                return null;

            int i = 0;
            var count = enumerable.Count();
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item, i, count);

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

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
            where T : class
        {
            if (source.IsNullOrEmpty() || item is null)
                return source;

            return source.Where(i => i != item);
        }

        public static IEnumerable<T> TakeHalf<T>(this IEnumerable<T> source, bool first = true)
        {
            if (source.IsNullOrEmpty())
                return Enumerable.Empty<T>();

            var count = source.Count();
            if (!first)
                source = source.Skip(count / 2);

            return source.Take(count % 2 == 0 ? count / 2 : count / 2 + 1);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random randomRange = null)
        {
            randomRange = randomRange ?? new Random();

            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = randomRange.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

    }
}
