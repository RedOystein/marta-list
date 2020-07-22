using System.Collections.Generic;

namespace Lameno.Models.Responses
{
    public class ListOutDto
    {
        public ListOutDto() { }

        public string Id { get; set; }
        public string Title { get; set; }
        public string ListTypeId { get; set; }
        public bool IsArchived { get; set; }
        public bool IsCompleted { get; set; }
        public string GroupId { get; set; }
        public List<ItemOutDto> Items { get; set; }
    }
}