using System.Collections.Generic;

namespace Lameno.Models.Responses
{
    public class GroupOutDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserOutDto> Users { get; set; }
        public List<string> ListIds { get; set; }
        public UserOutDto CreatedBy { get; set; }
    }
}