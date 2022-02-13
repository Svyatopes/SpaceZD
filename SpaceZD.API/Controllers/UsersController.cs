using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }


    [HttpGet]
    public ActionResult<List<UserModel>> GetUsers()
    {
        var userModel = _userService.GetList();
        var user = _mapper.Map<List<UserShortOutputModel>>(userModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Oh.....");
    }

    [HttpGet("{id}")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        var userModel = _userService.GetById(id);
        var user = _mapper.Map<UserFullOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    public ActionResult AddUser(UserRegisterInputModel userModel)
    {
        var user = _mapper.Map<UserModel>(userModel);
        var idAddedEntity = _userService.Add(user, userModel.Password);
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }

    [HttpPut("{id}")]
    public ActionResult EditUser(int id, UserUpdateInputModel user)
    {

        var userForEdit = _mapper.Map<UserModel>(user);
        _userService.Update(id, userForEdit);
        return Accepted();

    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userService.Update(id, true);
        return Accepted();

    }

    [HttpPatch("{id}")]
    public ActionResult RestoreUser(int id)
    {
        _userService.Update(id, false);
        return Accepted();

    }

}
