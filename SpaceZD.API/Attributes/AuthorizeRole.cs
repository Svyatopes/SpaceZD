using Microsoft.AspNetCore.Authorization;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Attributes;

public class AuthorizeRole : AuthorizeAttribute
{

    public AuthorizeRole(params Role[] roles) : base()
    {
        Roles = String.Join(",", Enum.GetNames(typeof(Role)));
    }

}

