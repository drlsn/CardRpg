using System.Collections.Generic;
using System.Linq;

namespace Core.Collections
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable is null || enumerable.Count() <= 0;
    }
}
