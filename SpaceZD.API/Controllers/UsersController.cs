using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<UserModel>> GetUsers()
    {
        return Ok(new List<UserModel> { new UserModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        return Ok(new UserModel());
    }


    [HttpPost]
    public ActionResult AddUser(UserModel user)
    {
        return StatusCode(StatusCodes.Status201Created, user);
    }

    [HttpPut("{id}")]
    public ActionResult EditUser(int id, UserModel user)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        return Accepted();
    }

}
