using HotChocolate.Types;
using Lameno.Models.Responses;

namespace Lameno.GraphQl
{
    public class GroupType : ObjectType<GroupOutDto>
    {
        protected override void Configure(IObjectTypeDescriptor<GroupOutDto> descriptor)
        {
            descriptor.Field(x => x.Id).Description("The ID of the group");
            descriptor.Field(x => x.Name).Description("Name of the group");
            descriptor.Field(x => x.Description).Description("Optional description of the group");
            descriptor.Field<GroupResolvers>(x => x.GetLists(default, default, default))
                .Description("Lists that are owned by this group")
                .Name("lists")
                .Type<ListType<Lameno.GraphQl.ListType>>();
            descriptor.Field(x => x.Users)
                .Description("Users that have access to current list")
                .Name("users")
                .Type<ListType<UserType>>();
        }
    }
}