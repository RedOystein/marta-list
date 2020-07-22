using HotChocolate.Types;
using Lameno.Models.Responses;

namespace Lameno.GraphQl
{
    public class UserType : ObjectType<UserOutDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserOutDto> descriptor)
        {
            descriptor.Field(x => x.Id).Description("ID of the user");
            descriptor.Field(x => x.Email).Description("Email of the user");
            descriptor.Field(x => x.Username).Description("Username of the user");
        }
    }
}