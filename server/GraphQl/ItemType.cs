using HotChocolate.Types;
using Lameno.Models.Responses;

namespace Lameno.GraphQl
{
    public class ItemType : ObjectType<ItemOutDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ItemOutDto> descriptor)
        {
            descriptor.Field(x => x.Id).Description("The ID of the list (GUID)");
            descriptor.Field(x => x.Title).Description("The title/name of the list item");
            descriptor.Field(x => x.ItemType).Description("The type of the item, i.e: produce, meat, etc..");
            descriptor.Field(x => x.IsCompleted).Description("Wether the item/task has been completed");
        }
    }
}