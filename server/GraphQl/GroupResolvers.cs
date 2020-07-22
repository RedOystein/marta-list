using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Lameno.Core;
using Lameno.Models.Responses;
using Microsoft.Extensions.Logging;

namespace Lameno.GraphQl
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