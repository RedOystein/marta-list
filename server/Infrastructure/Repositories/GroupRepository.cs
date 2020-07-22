using System.Collections.Generic;
using System.Threading.Tasks;
using MartaList.Infrastructure.Models;
using Microsoft.Azure.Cosmos.Table;
using MartaList.Extensions;
using System.Linq;

namespace MartaList.Infrastructure.Repositories
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetByUserId(string userId);
        Task<Group> Upsert(Group group);
        Task<List<Group>> GetGroups();
        Task<Group> GetGroup(string id);
    }

    public class GroupRepository : IGroupRepository
    {
        private readonly ITableClient<Group> repository;
        private readonly IUserRepository userRepository;

        public GroupRepository(
                ITableClient<Group> repository,
                IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }

        public async Task<List<Group>> GetByUserId(string userId)
        {
            var user = await userRepository.Get(userId);
            return await repository.GetItemsByTableQuery(GetGroupsQuery(user.GroupIdsList.SplitOrDefault()));
        }

        public Task<List<Group>> GetByListId(string listId)
        {
            var query = new TableQuery<Group>().Where(TableQueryExtensions.Contains(nameof(Group.ListIdsList), listId));
            return repository.GetItemsByTableQuery(query);
        }

        public async Task<Group> Upsert(Group group)
        {
            var createdGroup = await repository.UpsertItem(group);
            return createdGroup;
        }

        private static TableQuery<Group> GetGroupsQuery(List<string> ids)
        {
            var groupIds = ids.ToList();
            var query = TableQuery.GenerateFilterCondition(nameof(Group.RowKey), QueryComparisons.Equal, groupIds[0]);
            groupIds.RemoveAt(0);

            foreach (var groupId in groupIds)
                query = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(Group.RowKey), QueryComparisons.Equal, groupId),
                    TableOperators.Or,
                    query);

            return new TableQuery<Group>().Where(query);
        }

        public Task<List<Group>> GetGroups() => repository.GetItems(Group.EntityPartitionKey);

        public Task<Group> GetGroup(string id) => repository.GetItem(Group.EntityPartitionKey, id);
    }
}