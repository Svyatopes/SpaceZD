using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Models;
using SpaceZD.API.Models.Inputs;
using SpaceZD.BusinessLayer.Services;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public ActionResult Login([FromBody] AuthInputModel authInputModel)
    {
        var token = _authService.Login(authInputModel.Login, authInputModel.Password);

        return new JsonResult(token);
    }
        
}