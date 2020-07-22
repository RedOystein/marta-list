using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using MartaList.Core;
using MartaList.Models.Responses;
using Microsoft.Extensions.Logging;

namespace MartaList.GraphQl
{
    public class GroupResolvers
    {
        private readonly ILogger<GroupResolvers> logger;

        public GroupResolvers(ILogger<GroupResolvers> logger)
        {
            this.logger = logger;
        }

        public Task<List<ListOutDto>> GetLists(
            [Parent]GroupOutDto group,
            [Service]ListQuery listService,
            bool getArchieved = false)
                => getArchieved ?
                    listService.GetArchievedLists(group.Id) :
                    listService.GetLists(group.Id);
    }
}