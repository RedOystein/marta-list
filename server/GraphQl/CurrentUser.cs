using System;
using System.Collections.Generic;
using HotChocolate;

namespace MartaList.GraphQl
{
    public class CurrentUser
    {
        public string UserId { get; }
        public List<string> Claims { get; }

        public CurrentUser(string userId, List<string> claims)
        {
            UserId = userId;
            Claims = claims;
        }
    }

    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}