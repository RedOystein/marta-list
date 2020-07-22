using System.Collections.Generic;
using System.Threading.Tasks;
using MartaList.Infrastructure.Models;
using Microsoft.Azure.Cosmos.Table;

namespace MartaList.Infrastructure.Repositories
{
    public interface IItemRepository
    {
        Task Delete(ListItem item);
        Task<List<ListItem>> GetItems(string listId);
        Task<List<ListItem>> GetItems(List<string> listIds);
        Task<ListItem> Upsert(ListItem item);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly ITableClient<ListItem> client;

        public ItemRepository(ITableClient<ListItem> client)
            => this.client = client;

        public Task<List<ListItem>> GetItems(string listId)
            => client.GetItems(listId);

        public Task<List<ListItem>> GetItems(List<string> listIds)
            => client.GetItemsByTableQuery(GetListItemsQuery(listIds));

        public Task<ListItem> Upsert(ListItem item)
            => client.UpsertItem(item);

        public Task Delete(ListItem item)
            => client.DeleteItem(item);

        private static TableQuery<ListItem> GetListItemsQuery(List<string> listIds)
        {
            var query = TableQuery.GenerateFilterCondition(nameof(ListItem.PartitionKey), QueryComparisons.Equal, listIds[0]);
            listIds.RemoveAt(0);

            foreach (var listId in listIds)
                query = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(ListItem.PartitionKey), QueryComparisons.Equal, listId),
                    TableOperators.Or,
                    query);

            return new TableQuery<ListItem>().Where(query);
        }
    }
}