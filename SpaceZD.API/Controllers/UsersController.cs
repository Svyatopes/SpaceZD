using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Configuration;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUserService _userService;
    public UsersController(IUserService userService) 
    {
        _userService = userService;
    }

    //**************    NOT WORK YET      *******************
    
    [HttpGet]
    public ActionResult<List<UserModel>> GetUsers()
    {
        var userModel = _userService.GetList();
        var user = ApiMapper.GetInstance().Map<List<UserOutputModel>>(userModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Oh.....");
    }

    [HttpGet("{id}")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        var userModel = _userService.GetById(id);
        var user = ApiMapper.GetInstance().Map<UserOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    public ActionResult AddUser(UserModel userModel)
    {
        var user = ApiMapper.GetInstance().Map<UserModel>(userModel);
        var addEntity = _userService.Add(user);
        if (addEntity != null)
            return BadRequest("User doesn't add");
        else
            return StatusCode(StatusCodes.Status201Created, userModel);
    }

    [HttpPut("{id}")]
    public ActionResult EditUser(int id, UserModel user)
    {
        var userModel = _userService.GetById(id);
        var userForEdit = ApiMapper.GetInstance().Map<UserOutputModel>(userModel);
        return Ok();
        //var user = ApiMapper.GetInstance().Map<UserModel>(userModel);
        //return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        var userModel = _userService.GetById(id);
        var userForEdit = ApiMapper.GetInstance().Map<UserOutputModel>(userModel);
        return Ok();
    }

}
