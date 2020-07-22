using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MartaList.Extensions;
using Microsoft.Azure.Cosmos.Table;

namespace MartaList.Infrastructure.Models
{
    public class Group : TableEntity
    {
        private List<GroupUser> groupUsers;
        private List<string> listIds;

        public const string EntityPartitionKey = "Group";

        public Group()
        {
            PartitionKey = EntityPartitionKey;
            groupUsers = UsersJson.SerializeJsonListOrDefault<GroupUser>();
            listIds = ListIdsList.SplitOrDefault();
        }

        [IgnoreProperty]
        public string Id
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UsersJson { get; set; }
        public string CreatedByJson { get; set; }
        public string ListIdsList { get; set; }

        [IgnoreProperty]
        public GroupUser CreatedBy
        {
            get { return JsonSerializer.Deserialize<GroupUser>(CreatedByJson); }
            set { CreatedByJson = JsonSerializer.Serialize(value); }
        }

        public void AddUser(GroupUser user)
        {
            var users = JsonSerializer.Deserialize<List<GroupUser>>(UsersJson);
            users.Add(user);
            UsersJson = JsonSerializer.Serialize(users);
        }

        public void RemoveUser(string userId)
        {
            var users = JsonSerializer.Deserialize<List<GroupUser>>(UsersJson);
            users = users.Where(x => x.Id != userId).ToList();
            UsersJson = JsonSerializer.Serialize(users);
        }

        public void AddListId(string id) => ListIdsList += ListIdsList.IsEmpty() ? id : $",{id}";

        public void RemoveListId(string id) => ListIdsList.Replace(id, "").Trim(',');

        public bool ListIdExists(string id) => ListIdsList?.Contains(id) ?? false;
    }

    public class GroupUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsOwner { get; set; }
    }
}