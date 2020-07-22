using System.Threading.Tasks;
using HotChocolate;
using Lameno.Core;
using Lameno.Models.Responses;

namespace Lameno.GraphQl.Mutations
{
    public class Mutation
    {
        public Task<GroupOutDto> CreateGroup(
            [Service]IGroupService service,
            GroupOutDto group)
            => service.AddGroup(group);

        public async Task<string> RemoveGroupMember(
            [Service]IGroupService service,
            string groupId,
            string memberId)
        {
            await service.RemoveMember(groupId, memberId);
            return memberId;
        }

        public async Task<UserOutDto> AddGroupMember(
            [Service]IGroupService service,
            string groupId,
            UserOutDto member)
        {
            await service.AddMember(groupId, member);
            return member;
        }

        public Task<ApplicationUserOut> CreateUser(
            [CurrentUserGlobalState]CurrentUser user,
            [Service]ApplicationUserService service)
                => service.Create(new ApplicationUserOut{
                ExternalId = user.UserId,
                Username = "Marta",
                Email = "marta@yo.com"
            });

        public Task<ListOutDto> AddList(
            [CurrentUserGlobalState]CurrentUser user,
            [Service]ListService service,
            ListOutDto list)
                => service.AddList(list, user.UserId);

        public Task<ItemOutDto> AddItem(
            [CurrentUserGlobalState]CurrentUser user,
            [Service]ListService service,
            ItemOutDto item)
                => service.AddListItem(item, user.UserId);

        public Task<ItemOutDto> UpdateItem(
            [CurrentUserGlobalState]CurrentUser user,
            [Service]ListService service,
            ItemOutDto item)
                => service.UpdateListItem(item, user.UserId);
    }
}