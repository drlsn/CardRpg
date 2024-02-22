using System.Collections.Generic;
using System.Linq;

namespace Core.Collections
{
    public static class StringExtensions
    {
        public static string AggregateStrings(this IEnumerable<string> source, string separator = ", ") =>
           source.AggregateOrDefault((x,  y) => x + separator + y);

        public static string ToStr(this IEnumerable<char> source) =>
            new string(source.ToArray());

        public static string RemoveStartSubstring(this string str, string substring) =>
            str.RemoveStartSubstring(substring, out var _);

        public static string RemoveStartSubstring(this string str, string substring, out bool success)
        {
            success = false;
            if (substring.Length == 0)
            {
                success = true;
                return str;
            }

            if (!str.StartsWith(substring))
                return str;

            var previousCount = str.Length;
            var result = str.Remove(0, substring.Length);
            if (result.Length != previousCount)
            {
                success = true;
                return str;
            }

            return str;
        }
    }
}
