using Microsoft.Azure.Cosmos.Table;

namespace MartaList.Infrastructure.Models
{
    public class ListItem : TableEntity
    {
        public ListItem() { }

        [IgnoreProperty]
        public string Id
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        [IgnoreProperty]
        public string ListId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
        public string Title { get; set; }
        public string ItemType { get; set; }
        public bool IsCompleted { get; set; }
    }
}