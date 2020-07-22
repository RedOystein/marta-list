
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lameno.Infrastructure.Repositories;
using Lameno.Models.Responses;
using Lameno.Extensions;
using System;

namespace Lameno.Core
{
    public class ListService
    {
        private readonly IListRepository listRepository;
        private readonly IItemRepository itemRepository;
        private readonly IUserAuthorizationService userAuthorizationService;

        public ListService(
            IListRepository listRepository,
            IItemRepository itemRepository,
            IUserAuthorizationService userAuthorizationService)
        {
            this.listRepository = listRepository;
            this.itemRepository = itemRepository;
            this.userAuthorizationService = userAuthorizationService;
        }

        public async Task<ListOutDto> AddList(ListOutDto list, string userExternalId)
        {
            await userAuthorizationService.ValidateGroup(userExternalId, list.GroupId);
            list.Id = Guid.NewGuid().ToString();
            return (await listRepository
                .Upsert(list.AsDbModel()))
                .AsOutModel();
        }

        public async Task<ListOutDto> UpdateList(ListOutDto updatedList, string userExternalId)
        {
            await userAuthorizationService.ValidateGroup(userExternalId, updatedList.GroupId);

            return (await listRepository.Upsert(updatedList.AsDbModel())).AsOutModel();
        }

        public async Task<ItemOutDto> AddListItem(ItemOutDto item, string userExternalId)
        {
            await userAuthorizationService.ValidateList(userExternalId, item.ListId);

            item.Id = Guid.NewGuid().ToString();
            return (await itemRepository
                .Upsert(item.AsDbModel()))
                .AsOutModel();
        }

        public async Task<ItemOutDto> UpdateListItem(ItemOutDto updateItem, string userExternalId)
        {
            await userAuthorizationService.ValidateList(userExternalId, updateItem.ListId);

            return (await itemRepository
                .Upsert(updateItem.AsDbModel()))
                .AsOutModel();
        }
    }
}