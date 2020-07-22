using Microsoft.Azure.Cosmos.Table;

namespace Lameno.Infrastructure
{
    public static class TableQueryExtensions
    {
        public static string Contains(string propertyName, string value)
            => TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(propertyName, QueryComparisons.GreaterThanOrEqual, value),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(propertyName, QueryComparisons.LessThan, $"{value}0"));
    }
}