using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SpaceZD.API.Extensions;

public static class ControllerBaseExtensions
{
    public static int? GetUserId(this ControllerBase controllerBase)
    {
        var identity = controllerBase.HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null)
            return null;

        int userId;
        var userIdStr = identity.FindFirst(ClaimTypes.UserData)?.Value;
        var parsed = int.TryParse(userIdStr, out userId);
        if (!parsed)
            return null;
        return userId;
    }
}

