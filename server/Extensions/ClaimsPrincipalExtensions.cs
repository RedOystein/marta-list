using System.Collections.Generic;
using System.Security.Claims;

namespace Lameno.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
            => user.FindFirst(x => x.Type == "user_id").Value;
    }

    public static class UserContextExtensions
    {
        public static string GetUserId(this IDictionary<string, object> userContext)
            => ((ClaimsPrincipal)userContext).FindFirst(x => x.Type == "user_id").Value;
    }
}