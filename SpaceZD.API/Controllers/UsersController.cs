using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

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
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<UserModel>> GetUsers()
    {
        var userModel = _userService.GetList();
        var user = _mapper.Map<List<UserShortOutputModel>>(userModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Oh.....");
    }


    [HttpGet("id/persons")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult<List<PersonModel>> GetListPersonsFromUser()
    {
        var login = HttpContext.User.Identity.Name;
        var userId = _userService.GetByLogin(login).Id;
        var personModel = _userService.GetListUserPersons(userId);
        var user = _mapper.Map<List<PersonOutputModel>>(personModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Oh.....");
    }


    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<UserModel> GetUserById(int id)
    {

        var userModel = _userService.GetById(id);
        var user = _mapper.Map<UserFullOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpGet("by-login")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult<UserModel> GetUserByLogin()
    {
        var login = HttpContext.User.Identity.Name;
        var userModel = _userService.GetByLogin(login);
        var user = _mapper.Map<UserFullOutputModel>(userModel);
        return Ok(user);

    }


    [HttpPost]
    public ActionResult AddUser([FromBody] UserRegisterInputModel userModel)
    {
        var user = _mapper.Map<UserModel>(userModel);
        var idAddedEntity = _userService.Add(user, userModel.Password);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut]
    [Authorize]
    public ActionResult EditUser(UserUpdateInputModel user)
    {
        var login = HttpContext.User.Identity.Name;
        var userModel = _userService.GetByLogin(login);
        var userForEdit = _mapper.Map<UserModel>(user);
        _userService.Update(userModel.Id, userForEdit);
        return Accepted();

    }


    [HttpDelete]
    [Authorize]
    public ActionResult DeleteUser()
    {
        var login = HttpContext.User.Identity.Name;
        var userModel = _userService.GetByLogin(login);
        _userService.Update(userModel.Id, true);
        return Accepted();

    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreUser(int id)
    {
        _userService.Update(id, false);
        return Accepted();

    }

}
