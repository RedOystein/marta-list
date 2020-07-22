namespace MartaList.Models.Responses
{
    public class ItemOutDto
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Title { get; set; }
        public string ItemType { get; set; }
        public bool IsCompleted { get; set; }

        public void Completed()
            => IsCompleted = true;

        public void InComplete()
            => IsCompleted = false;
    }
}