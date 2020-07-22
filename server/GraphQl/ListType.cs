using HotChocolate.Types;
using MartaList.Models.Responses;

namespace MartaList.GraphQl
{
    public class ListType : ObjectType<ListOutDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ListOutDto> descriptor)
        {
            descriptor.Field(x => x.Id).Description("The ID of the list (GUID)");
            descriptor.Field(x => x.Title).Description("The title/name of the list");
            descriptor.Field(x => x.ListTypeId).Description("Type of list, currently the only type is ShoppingList");
            descriptor.Field(x => x.IsArchived).Description("Wether the list is archieved or not");
            descriptor.Field(x => x.IsCompleted).Description("Not sure if this is needed at all");
            // descriptor.Field("group")
            //     .Description("The group this list is owned by")
            //     .Type<GroupType>();
            descriptor.Field(x => x.Items)
                .Description("Items in the list")
                .Name("items")
                .Type<ListType<ItemType>>();
            // descriptor.Field("lists")
            //     .Description("Sub-lists that belong to the list")
            //     .Type<ListType<MartaList.GraphQl.ListType>>();
        }
    }
}