using AutoMapper;
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
[AuthorizeRole(Role.Admin, Role.User)]

public class PersonsController : ControllerBase
{
    
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public PersonsController(IPersonService personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;

    }
    [HttpGet]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<PersonModel>> GetPersons()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var personModel = _personService.GetList(userId.Value);
        var user = _mapper.Map<List<PersonOutputModel>>(personModel);
        if (user != null)
            return Ok(user);
        return BadRequest();
    }

    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<PersonModel> GetPersonById(int id)
    {

        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userModel = _personService.GetById(id, userId.Value);
        var user = _mapper.Map<PersonOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("Person doesn't exist");
    }


    [HttpGet("by-user-login")]
    public ActionResult<PersonModel> GetPersonByUserId()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var personModel = _personService.GetByUserId(userId.Value);
        var user = _mapper.Map<List<PersonOutputModel>>(personModel);
        return Ok(user);
       
    }


    [HttpPost]
    public ActionResult AddPerson([FromBody] PersonInputModel personModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var person = _mapper.Map<PersonModel>(personModel);
        var idAddedEntity = _personService.Add(person, userId.Value);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    public ActionResult EditPerson(int id, PersonInputModel person)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var personForEdit = _mapper.Map<PersonModel>(person);
        _personService.Update(id, personForEdit, userId.Value);
        return Accepted();

    }
    

    [HttpDelete("{id}")]
    public ActionResult DeletePerson(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _personService.Delete(id, userId.Value);
        return Accepted();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestorePerson(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _personService.Restore(id, userId.Value);
        return Accepted();
    }
}