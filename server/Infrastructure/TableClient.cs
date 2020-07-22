using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace Lameno.Infrastructure
{

    public interface ITableClient<TEntity> where TEntity : TableEntity, new()
    {
        Task<TEntity> GetItem(string partitionKey, string rowKey);
        Task<TEntity> UpsertItem(TEntity item);
        Task<List<TEntity>> GetItems();
        Task<List<TEntity>> GetItems(string partitionKey);
        Task<List<TEntity>> GetItemsByTableQuery(TableQuery<TEntity> query);
        Task DeleteItem(TEntity item);
    }

    public class TableClient<TEntity> : ITableClient<TEntity> where TEntity : TableEntity, new()
    {
        private readonly string CollectionId = typeof(TEntity).Name;
        private readonly CloudTable table;

        public TableClient(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("AzureTable");
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient(new TableClientConfiguration());
            table = client.GetTableReference(CollectionId);
            table.CreateIfNotExists();
        }

        public async Task<List<TEntity>> GetItemsByTableQuery(TableQuery<TEntity> query)
        {
            var results = new List<TEntity>();
            TableContinuationToken token = null;
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                results.AddRange(segment.Results);
            } while (token != null);

            return results;
        }

        public async Task<TEntity> UpsertItem(TEntity entity)
            => (await table.ExecuteAsync(TableOperation.InsertOrReplace(entity))).Result as TEntity;

        public async Task DeleteItem(TEntity entity)
            => await table.ExecuteAsync(TableOperation.Delete(entity));

        public async Task<TEntity> GetItem(string partitionKey, string rowKey)
        {
            var result = await table.ExecuteAsync(TableOperation.Retrieve<TEntity>(partitionKey, rowKey));
            return result.Result as TEntity;
        }

        public Task<List<TEntity>> GetItems(string partitionKey)
            => GetItemsByTableQuery(new TableQuery<TEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)));

        public async Task<List<TEntity>> GetItems()
        {
            var results = new List<TEntity>();
            TableContinuationToken token = null;
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(new TableQuery<TEntity>(), token);
                results.AddRange(segment.Results);
            } while (token != null);
            return results;
        }
    }
}