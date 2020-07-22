using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MartaList.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool HasValue(this string s) => !IsEmpty(s);

        public static List<string> SplitOrDefault(this string s, char sep = ',')
            => s.IsEmpty() ? new List<string>() : s.Split(sep).ToList();

        public static List<T> SerializeJsonListOrDefault<T>(this string s) => s.IsEmpty() ? new List<T>() : JsonSerializer.Deserialize<List<T>>(s);
    }
}