using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Maths
{
    public static class NumericExtensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
                return min;
            else
            if (val.CompareTo(max) > 0)
                return max;

            return val;
        }

        public static int GetPercent<T>(this IEnumerable<T> source, int index)
        {
            var count = source.Count();
            var percent = (index / (float) count) * 100;
            return (int) percent;
        }
    }
}
