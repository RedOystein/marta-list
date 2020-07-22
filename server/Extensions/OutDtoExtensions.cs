using System.Linq;
using Lameno.Infrastructure.Models;
using Lameno.Models.Responses;

namespace Lameno.Extensions
{
    public static class OutDtoExtensions
    {
        public static Group AsDbModel(this GroupOutDto group)
            => new Group
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                CreatedBy = group.CreatedBy.AsDbModel(),
                UsersJson = group.Users.Select(AsDbModel).ToJson(),
                ListIdsList = group.ListIds.JoinNullable()
            };

        public static List AsDbModel(this ListOutDto list)
            => new List
            {
                Id = list.Id,
                GroupId = list.GroupId,
                Title = list.Title,
                ListTypeId = list.ListTypeId,
                IsArchieved = list.IsArchived,
                IsMultiList = false
            };

        public static ListItem AsDbModel(this ItemOutDto item)
            => new ListItem
            {
                Id = item.Id,
                ListId = item.ListId,
                Title = item.Title,
                ItemType = item.ItemType,
                IsCompleted = item.IsCompleted
            };

        public static User AsDbModel(this ApplicationUserOut user)
            => new User
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                ExternalId = user.ExternalId,
                GroupIdsList = user.GroupIds.JoinNullable()
            };

        public static GroupUser AsDbModel(this UserOutDto user)
            => new GroupUser
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
    }
}