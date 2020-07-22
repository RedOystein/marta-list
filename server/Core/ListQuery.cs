using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lameno.Extensions;
using Lameno.Infrastructure.Repositories;
using Lameno.Models.Responses;

namespace Lameno.Core
{
    public class ListQuery
    {
        private readonly IListRepository listRepository;

        public ListQuery(IListRepository listRepository)
            => this.listRepository = listRepository;

        public async Task<List<ListOutDto>> GetArchievedLists(string groupId)
            => (await listRepository.GetArchieved(groupId))?
                    .OrderByDescending(list => list.Timestamp)
                    .Select(list => list.AsOutModel()).ToList() ??
                new List<ListOutDto>();

        public async Task<List<ListOutDto>> GetLists(string groupId)
            => (await listRepository.Get(groupId))?
                    .OrderByDescending(list => list.Timestamp)
                    .Select(list => list.AsOutModel()).ToList() ??
                new List<ListOutDto>();
    }
}