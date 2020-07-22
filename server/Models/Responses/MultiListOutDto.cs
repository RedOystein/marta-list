using System.Collections.Generic;

namespace Lameno.Models.Responses
{
    public class MultiListOutDto
    {
        public MultiListOutDto() { }

        public MultiListOutDto(string id, string title, string listTypeId)
        {
            Id = id;
            Title = title;
            ListTypeId = listTypeId;
            IsArchived = false;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string ListTypeId { get; set; }
        public bool IsArchived { get; set; }
        public string GroupId { get; set; }
        public List<ListOutDto> Lists { get; set; }

    }
}