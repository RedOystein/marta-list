using System.Collections.Generic;

namespace MartaList.Models.Responses
{
    public class ApplicationUserOut
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ExternalId { get; set; }
        public List<string> GroupIds { get; set; }
    }
}