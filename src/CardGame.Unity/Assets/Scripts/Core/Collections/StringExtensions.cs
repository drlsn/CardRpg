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
    }
}
