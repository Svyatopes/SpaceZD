using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

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
    [SwaggerOperation(Summary ="Get all users")]
    [ProducesResponseType(typeof(List<UserShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get all deleted users")]
    [ProducesResponseType(typeof(List<UserShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get user by id")]
    [ProducesResponseType(typeof(UserFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get inforation about youself")]
    [ProducesResponseType(typeof(UserFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Add new user")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)] 
    public ActionResult AddUser([FromBody] UserRegisterInputModel userModel)
    {

        var user = _mapper.Map<UserModel>(userModel);
        var idAddedEntity = _userService.Add(user, userModel.Password);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    [Authorize]
    [SwaggerOperation(Summary = "Edit information about youself")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult EditUser(int id, [FromBody] UserUpdateInputModel user)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userForEdit = _mapper.Map<UserModel>(user);
        _userService.Update(userId.Value, id, userForEdit);
        return NoContent();

    }


    [HttpPut("{id}/role")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Edit role in user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult EditRole(int id, [FromBody] RoleInputModel roleModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var role = _mapper.Map<Role>(roleModel);


        _userService.UpdateRole(id, role, userId.Value);
        return NoContent();

    }


    [HttpPut("{id}/password")]
    [Authorize]
    [SwaggerOperation(Summary = "Edit your passwort")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult EditPassword([FromBody] UserUpdatePasswordInputModel userModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.UpdatePassword(userModel.PasswordOld, userModel.PasswordNew, userId.Value);
        return NoContent();


    }


    [HttpDelete("{id}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete user by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult DeleteUser(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.Delete(id, userId.Value);
        return NoContent();

    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restore user by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult RestoreUser(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _userService.Restore(id, userId.Value);
        return NoContent();

    }

}
