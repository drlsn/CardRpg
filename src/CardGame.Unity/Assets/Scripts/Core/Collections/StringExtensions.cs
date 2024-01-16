using System.Collections.Generic;

namespace Core.Collections
{
    public static class StringExtensions
    {
        public static string AggregateStrings(this IEnumerable<string> source, string separator = ", ") =>
           source.AggregateOrDefault((x,  y) => x + separator + y);
    }
}
