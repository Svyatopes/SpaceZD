using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
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
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userModel = _userService.GetList(userId.Value);
        var users = _mapper.Map<List<UserShortOutputModel>>(userModel);
        if (users != null)
            return Ok(users);
        return BadRequest("Oh.....");
    }


    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<UserModel>> GetUsersDelete()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userModel = _userService.GetListDelete(userId.Value);
        var users = _mapper.Map<List<UserShortOutputModel>>(userModel);
        if (users != null)
            return Ok(users);
        return BadRequest("Oh.....");
    }


    
    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<UserModel> GetUserById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userModel = _userService.GetById(id, userId.Value);
        var user = _mapper.Map<UserFullOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpGet("by-login")]
    [Authorize]
    public ActionResult<UserModel> GetUserByLogin()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var login = HttpContext.User.Identity.Name;
        var userModel = _userService.GetByLogin(login, userId.Value);
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


    [HttpPut("{id}")]
    [Authorize]
    public ActionResult EditUser(int id, [FromBody] UserUpdateInputModel user)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userForEdit = _mapper.Map<UserModel>(user);
        _userService.Update(id, userForEdit, userId.Value);
        return Accepted();

    }


    [HttpPut("{id}/role")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult EditRole(int id, [FromBody] RoleInputModel roleModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var role = _mapper.Map<Role>(roleModel);


        _userService.UpdateRole(id, role, userId.Value);
        return Accepted();

    }


    [HttpPut("{id}/password")]
    [Authorize]
    public ActionResult EditPassword([FromBody] UserUpdatePasswordInputModel userModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.UpdatePassword(userModel.PasswordOld, userModel.PasswordNew, userId.Value);
        return Accepted();


    }


    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult DeleteUser(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.Delete(id, userId.Value);
        return Accepted();

    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreUser(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.Restore(id, userId.Value);
        return Accepted();

    }

}
