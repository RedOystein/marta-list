using HotChocolate.Types;
using Lameno.Core;

namespace Lameno.GraphQl.Queries
{
    public class GroupQueryType : ObjectType<GroupQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<GroupQuery> descriptor)
        {
            descriptor.Field(t => t.GetGroups())
                .Type<ListType<GroupType>>()
                .Name("allGroups")
                .Description("Get all groups");

            descriptor.Field(t => t.GetGroups(default))
                .Type<ListType<GroupType>>()
                .Name("groups")
                .Description("Get all the users groups");

            descriptor.Field(t => t.GetGroup(default))
                .Type<GroupType>();

        }
    }
}