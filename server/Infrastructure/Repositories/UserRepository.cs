using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Lameno.Extensions;
using Lameno.Infrastructure.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace Lameno.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task Delete(User user);
        Task<User> Get(string id);
        Task<User> GetByExternalId(string externalId);
        Task<List<User>> GetUsers(string[] ids);
        Task<User> Upsert(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ITableClient<User> client;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(ITableClient<User> client, ILogger<UserRepository> logger)
        {
            this.client = client;
            this.logger = logger;
        }


        public Task<User> Upsert(User user)
        {
            logger.LogDebug($"Upserting: {JsonSerializer.Serialize(user)}");
            return client.UpsertItem(user);
        }

        public Task<User> Get(string id)
            => client.GetItem(User.EntityPartitionKey, id);


        public async Task<User> GetByExternalId(string externalId)
        {
            var query = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(User.PartitionKey), QueryComparisons.Equal, User.EntityPartitionKey),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(User.ExternalId), QueryComparisons.Equal, externalId)
            );

            var user = await client.GetItemsByTableQuery(new TableQuery<User>().Where(query));
            return user.HasElements() ? user[0] : null;
        }

        public Task Delete(User user)
            => client.DeleteItem(user);

        public Task<List<User>> GetUsers(string[] ids)
        {
            // TODO: add empty check after extensions are added 
            var userIds = ids.ToList();
            var query = GetUsersQuery(userIds);
            return client.GetItemsByTableQuery(query);
        }

        private static TableQuery<User> GetUsersQuery(List<string> userIds)
        {
            var query = TableQuery.GenerateFilterCondition(nameof(User.RowKey), QueryComparisons.Equal, userIds[0]);
            userIds.RemoveAt(0);

            foreach (var userId in userIds)
                query = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(User.RowKey), QueryComparisons.Equal, userId),
                    TableOperators.Or,
                    query);

            return new TableQuery<User>().Where(query);
        }
    }
}