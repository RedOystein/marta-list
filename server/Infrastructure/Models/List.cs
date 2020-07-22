using System.Collections.Generic;
using Lameno.Extensions;
using Microsoft.Azure.Cosmos.Table;

namespace Lameno.Infrastructure.Models
{
    public class List : TableEntity
    {
        [IgnoreProperty]
        public string Id
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        [IgnoreProperty]
        public string GroupId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public string Title { get; set; }
        public string ListTypeId { get; set; }
        public bool IsArchieved { get; set; }
        public string ListIdsList { get; set; }
        public bool IsMultiList { get; set; }

        [IgnoreProperty]
        public List<ListItem> Items { get; set; }

        public void AddListId(string id) => ListIdsList += ListIdsList.IsEmpty() ? id : $",{id}";

        public void RemoveListId(string id) => ListIdsList.Replace(id, "").Trim(',');
    }
}