using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lameno.Extensions;
using Lameno.Infrastructure.Models;
using Microsoft.Azure.Cosmos.Table;

namespace Lameno.Infrastructure.Repositories
{
    public interface IListRepository
    {
        Task<List<List>> Get(string groupId);
        Task<List<List>> GetArchieved(string groupId);
        Task<List> Upsert(List list);
    }

    public class ListRepository : IListRepository
    {
        private readonly ITableClient<List> client;
        private readonly ITableClient<Group> groupClient;
        private readonly IItemRepository itemsRepository;

        public ListRepository(
                    ITableClient<List> client,
                    ITableClient<Group> groupClient,
                    IItemRepository itemsRepository)
        {
            this.client = client;
            this.groupClient = groupClient;
            this.itemsRepository = itemsRepository;
        }

        public async Task<List<List>> Get(string groupId)
        {
            var query = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, groupId),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("IsArchieved", QueryComparisons.Equal, false));

            var lists = await client.GetItemsByTableQuery(new TableQuery<List>().Where(query));

            if (lists.IsEmpty())
                return new List<List>();

            var items = await itemsRepository.GetItems(lists.Select(x => x.Id).ToList());

            return lists.Select(list =>
            {
                list.Items = items.Where(item => item.ListId == list.Id).ToList();
                return list;
            }).ToList();
        }

        public Task<List<List>> GetArchieved(string groupId)
        {
            var query = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, groupId),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("IsArchieved", QueryComparisons.Equal, true));

            return client.GetItemsByTableQuery(new TableQuery<List>().Where(query));
        }

        public async Task<List> Upsert(List list)
        {
            var group = await groupClient.GetItem(Group.EntityPartitionKey, list.GroupId);
            var upsertedList = await client.UpsertItem(list);

            if (!group.ListIdExists(list.Id))
            {
                group.AddListId(list.Id);
                await groupClient.UpsertItem(group);
            }

            return upsertedList;
        }
    }
}