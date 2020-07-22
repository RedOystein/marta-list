using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lameno.Exceptions;
using Lameno.Extensions;
using Lameno.Infrastructure.Repositories;
using Lameno.Models.Responses;

namespace Lameno.Core
{
    public interface IGroupService
    {
        Task<GroupOutDto> AddGroup(GroupOutDto group);
        Task AddMember(string groupId, UserOutDto member);
        Task RemoveMember(string groupId, string memberId);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepository userRepository;

        public GroupService(
                IGroupRepository groupRepository,
                IUserRepository userRepository)
        {
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        public async Task<GroupOutDto> AddGroup(GroupOutDto group)
        {
            Validate(group);

            group.Id = Guid.NewGuid().ToString();
            var createdByUser = await userRepository.Get(group.CreatedBy.Id);

            if (createdByUser == null)
                throw new UserForbiddenException(group.CreatedBy.Id, group.CreatedBy.Username);

            var createdGroup = (await groupRepository.Upsert(group.AsDbModel())).AsOutModel();
            createdByUser.AddGroupId(createdGroup.Id);
            await userRepository.Upsert(createdByUser);

            return createdGroup;
        }

        public async Task AddMember(string groupId, UserOutDto member)
        {
            var group = (await groupRepository.GetGroup(groupId)).AsOutModel();

            if (group.Users.Select(x => x.Id).Contains(member.Id))
                throw new ArgumentException($"member {member.Id} already exists in {groupId}");

            group.Users.Add(member);

            await groupRepository.Upsert(group.AsDbModel());
        }

        public async Task RemoveMember(string groupId, string memberId)
        {
            var group = (await groupRepository.GetGroup(groupId)).AsOutModel();

            if (!group.Users.Select(x => x.Id).Contains(memberId))
                return;

            group.Users = group.Users.Where(x => x.Id != memberId).ToList();

            await groupRepository.Upsert(group.AsDbModel());
        }


        private Task NotifyUsers(GroupOutDto createdGroup)
        {
            var usersToNotify = createdGroup.Users.Where(user => user.Id != createdGroup.CreatedBy.Id);
            if (usersToNotify.IsEmpty())
                return Task.CompletedTask;

            // TODO: Ths should call notification service
            return Task.CompletedTask;
        }

        private void Validate(GroupOutDto group)
        {
            if (group.Name.IsEmpty())
                throw new ArgumentException($"{nameof(GroupOutDto.Name)} cannot be null or empty");

            if (group.Users.IsEmpty())
                throw new ArgumentException($"{nameof(GroupOutDto.Users)} must have one or more user");
        }
    }
}