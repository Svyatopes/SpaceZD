using AutoMapper;
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
        var user = _mapper.Map<List<UserOutputModel>>(userModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Oh.....");
    }

    [HttpGet("{id}")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        var userModel = _userService.GetById(id);
        var user = _mapper.Map<UserOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    public ActionResult AddUser(UserRegisterInputModel userModel)
    {
        var user = _mapper.Map<UserModel>(userModel);
        var addEntity = _userService.Add(user);
        if (addEntity != null)
            return BadRequest("User doesn't add");
        else
            return StatusCode(StatusCodes.Status201Created, userModel);
    }

    [HttpPut("{id}")]
    public ActionResult EditUser(int id, UserUpdateInputModel user)
    {
        //var userModel = _userService.GetById(id);
        //var userForEdit = _mapper.Map<UserModel>(userModel);
        //_userService.Update(userModel, userForEdit);
        //if (userUpdate)
            return Ok();
        
            return BadRequest("User doesn't update");

    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id, bool isDeleted)
    {
        var userModel = _userService.GetById(id);
        var userForEdit = _mapper.Map<UserModel>(userModel);
        //var userUpdate = _userService.Update(id, isDeleted);
        //if (userUpdate)
        //    return Ok();
        //else
            return BadRequest("User doesn't delete");
    }

}
