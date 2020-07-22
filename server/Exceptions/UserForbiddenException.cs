namespace MartaList.Exceptions
{
    public class UserForbiddenException : System.Exception
    {
        public string GroupId { get; }
        public string ListId { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UserForbiddenException(string userId, string userName,
            string groupId = null, string listId = null)
        {
            UserId = userId;
            UserName = userName;
            GroupId = groupId;
            ListId = listId;
        }
    }
}