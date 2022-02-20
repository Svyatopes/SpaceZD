using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var personModel = _personService.GetList();
        var user = _mapper.Map<List<PersonOutputModel>>(personModel);
        if (user != null)
            return Ok(user);
        return BadRequest();
    }

    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<PersonModel> GetPersonById(int id)
    {

        var userModel = _personService.GetById(id);
        var user = _mapper.Map<PersonOutputModel>(userModel);
        if (user != null)
            return Ok(user);
        else
            return BadRequest("Person doesn't exist");
    }


    [HttpGet("by-user-login")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult<PersonModel> GetPersonByUserLogin()
    {
        var login = HttpContext.User.Identity.Name;
        var personModel = _personService.GetByUserLogin(login);
        var user = _mapper.Map<List<PersonOutputModel>>(personModel);
        return Ok(user);
       
    }


    [HttpGet("with-tickets/{id}")]
    public ActionResult<PersonModel> GetPersonByIdWithTickets(int id)
    {
        return NotFound("Can't find(((((");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult AddPerson([FromBody] PersonInputModel personModel)
    {
        var login = HttpContext.User.Identity.Name;
        var person = _mapper.Map<PersonModel>(personModel);
        var idAddedEntity = _personService.Add(person, login);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult EditPerson(int id, PersonInputModel person)
    {
        var login = HttpContext.User.Identity.Name;
        var personForEdit = _mapper.Map<PersonModel>(person);
        _personService.Update(id, personForEdit, login);
        return Accepted();

    }
    


    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult DeletePerson(int id)
    {
        var login = HttpContext.User.Identity.Name;
        _personService.Update(id, true, login);
        return Accepted();
    }

    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestorePerson(int id)
    {
        var login = HttpContext.User.Identity.Name;
        _personService.Update(id, false, login);
        return Accepted();
    }
}