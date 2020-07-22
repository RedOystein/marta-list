using Lameno.Extensions;
using Microsoft.Azure.Cosmos.Table;

namespace Lameno.Infrastructure.Models
{
    public class User : TableEntity
    {
        public const string EntityPartitionKey = "User";

        public User()
            => PartitionKey = EntityPartitionKey;

        [IgnoreProperty]
        public string Id
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ExternalId { get; set; }
        public string GroupIdsList { get; set; }

        public void AddGroupId(string id) => GroupIdsList += GroupIdsList.IsEmpty() ? id : $",{id}";

        public void RemoveGroupId(string id) => GroupIdsList.Replace(id, "").Trim(',');
    }
}