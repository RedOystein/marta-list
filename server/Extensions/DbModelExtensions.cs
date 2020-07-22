using System.Linq;
using Lameno.Infrastructure.Models;
using Lameno.Models.Responses;

namespace Lameno.Extensions
{
    public static class DbModelExtensions
    {
        public static GroupOutDto AsOutModel(this Group group)
            => new GroupOutDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Users = group.UsersJson.SerializeJsonListOrDefault<GroupUser>().Select(AsOutModel).ToList(),
                ListIds = group.ListIdsList.SplitOrDefault(),
                CreatedBy = group.CreatedBy.AsOutModel()
            };

        public static MultiListOutDto AsOutMultiListModel(this List list)
            => new MultiListOutDto
            {
                Id = list.Id,
                Title = list.Title,
                ListTypeId = list.ListTypeId,
                IsArchived = list.IsArchieved,
                GroupId = list.GroupId,
            };

        public static ListOutDto AsOutModel(this List list)
            => new ListOutDto
            {
                Id = list.Id,
                Title = list.Title,
                ListTypeId = list.ListTypeId,
                IsArchived = list.IsArchieved,
                GroupId = list.GroupId,
                Items = list.Items?.Select(AsOutModel).ToList()
            };

        public static ItemOutDto AsOutModel(this ListItem item)
            => new ItemOutDto
            {
                Id = item.Id,
                ListId = item.ListId,
                Title = item.Title,
                ItemType = item.ItemType,
                IsCompleted = item.IsCompleted
            };

        public static ApplicationUserOut AsOutModel(this User user)
            => new ApplicationUserOut
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                ExternalId = user.ExternalId,
                GroupIds = user.GroupIdsList.SplitOrDefault()
            };

        public static UserOutDto AsOutModel(this GroupUser user)
            => new UserOutDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
    }
}