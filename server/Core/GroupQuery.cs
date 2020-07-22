using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lameno.Extensions;
using Lameno.Infrastructure.Repositories;
using Lameno.Models.Responses;

namespace Lameno.Core
{
    public class GroupQuery
    {
        private readonly IGroupRepository repository;

        public GroupQuery(IGroupRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<GroupOutDto>> GetGroups() => (await repository.GetGroups()).Select(x => x.AsOutModel()).ToList();


        public async Task<List<GroupOutDto>> GetGroups(string userId)
        {
            if (userId.IsEmpty())
                throw new ArgumentException(nameof(userId));

            return (await repository.GetByUserId(userId))?
                        .Select(x => x.AsOutModel())
                        .ToList() ??
                    new List<GroupOutDto>();
        }

        public async Task<GroupOutDto> GetGroup(string groupId)
        {
            if (groupId.IsEmpty())
                throw new ArgumentException(nameof(groupId));

            return (await repository.GetGroup(groupId)).AsOutModel();
        }

    }
}