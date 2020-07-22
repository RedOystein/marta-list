using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MartaList.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list) => list == null || list.Count() == 0;

        public static bool HasElements<T>(this IEnumerable<T> list) => !IsEmpty(list);
        public static string JoinNullable<T>(this IEnumerable<T> values, char separator = ',')
            => values.HasElements() ? string.Join(separator, values) : null;

        public static string ToJson<T>(this IEnumerable<T> values) => JsonSerializer.Serialize(values);
    }
}