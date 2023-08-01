using System;
using System.Collections.Generic;

namespace Core.Collections
{
    public static class ListExtensions
    {
        public static void RemoveIf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (!predicate(item))
                    continue;

                list.RemoveAt(i);
            }
        }
    }
}
