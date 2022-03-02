using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get all persons")]
    [ProducesResponseType(typeof(List<PersonOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get person by id")]
    [ProducesResponseType(typeof(PersonOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<PersonModel> GetPersonById(int id)
    {

        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var userModel = _personService.GetById(id, userId.Value);
        var user = _mapper.Map<PersonOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        return BadRequest("Person doesn't exist");
    }


    [HttpGet("by-user-id")]
    [AuthorizeRole(Role.User)]
    [SwaggerOperation(Summary = "Get persons by user id")]
    [ProducesResponseType(typeof(List<PersonOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<PersonModel> GetPersonsByUserId()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var personModel = _personService.GetByUserId(userId.Value);
        var persons = _mapper.Map<List<PersonOutputModel>>(personModel);
        if (persons != null)
            return Ok(persons);
        return BadRequest("Persons doesn't exist");

    }


    [HttpPost]
    [SwaggerOperation(Summary = "Add new person")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult AddPerson([FromBody] PersonInputModel personModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var person = _mapper.Map<PersonModel>(personModel);
        var idAddedEntity = _personService.Add(userId.Value, person);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Edit person by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult EditPerson(int id, PersonInputModel person)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var personForEdit = _mapper.Map<PersonModel>(person);
        _personService.Update(userId.Value, id, personForEdit);
        return NoContent();

    }


    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete person by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult DeletePerson(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _personService.Delete(id, userId.Value);
        return NoContent();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restore person by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult RestorePerson(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _personService.Restore(id, userId.Value);
        return NoContent();
    }
}