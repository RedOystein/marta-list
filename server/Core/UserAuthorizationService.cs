
using System.Linq;
using System.Threading.Tasks;
using MartaList.Infrastructure.Repositories;
using MartaList.Exceptions;

namespace MartaList.Core
{
    public interface IUserAuthorizationService
    {
        Task ValidateList(string userExternalId, string listId);
        Task ValidateGroup(string userExternalId, string groupId);
    }

    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IUserRepository userRepository;
        private readonly IGroupRepository groupRepository;

        public UserAuthorizationService(
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            this.userRepository = userRepository;
            this.groupRepository = groupRepository;
        }

        public async Task ValidateList(string userExternalId, string listId)
        {
            var user = await userRepository.GetByExternalId(userExternalId);
            var groups = await groupRepository.GetByUserId(user.Id);

            if (!groups.Any(group => group.ListIdsList.Contains(listId)))
                throw new UserForbiddenException(user.Id, user.Username, listId: listId);
        }

        public async Task ValidateGroup(string userExternalId, string groupId)
        {
            var user = await userRepository.GetByExternalId(userExternalId);

            if (!user.GroupIdsList.Contains(groupId))
                throw new UserForbiddenException(user.Id, user.Username, groupId: groupId);
        }
    }
}