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
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;

    public TicketsController(ITicketService ticketService, IMapper mapper)
    {
        _ticketService = ticketService;
        _mapper = mapper;

    }

    [HttpGet]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<TicketModel>> GetTickets()
    {
        var ticketModel = _ticketService.GetList();
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest("Oh.....");
    }
    
    
    [HttpGet("delete")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<TicketModel>> GetTicketsDelete()
    {
        var ticketModel = _ticketService.GetListDeleted();
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest();
    }

    [HttpGet("by-order/{orderId}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult<List<TicketModel>> GetTicketByOrderId(int orderId)
    {
        var login = HttpContext.User.Identity.Name;
        var ticketModel = _ticketService.GetListByOrderId(orderId, login);
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest("Oh.....");
    }

    
    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult<TicketModel> GetTicketById(int id)
    {
        var login = HttpContext.User.Identity.Name;
        var ticketModel = _ticketService.GetById(id, login);
        var ticket = _mapper.Map<TicketOutputModel>(ticketModel);
        if (ticket != null)
            return Ok(ticket);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult AddTicket(TicketCreateInputModel ticketModel)
    {
        var login = HttpContext.User.Identity.Name; 
        var ticket = _mapper.Map<TicketModel>(ticketModel);
        var idAddedEntity = _ticketService.Add(ticket, login);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }

    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult EditTicket(int id, TicketUpdateInputModel ticketModel)
    {
        var login = HttpContext.User.Identity.Name;
        var ticket = _mapper.Map<TicketModel>(ticketModel);
        _ticketService.Update(id, ticket, login);
        return Accepted();
    }


    [HttpPut("price/{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult EditPriceTicket(int id, TicketUpdatePriceInputModel ticketModel)
    {
        var login = HttpContext.User.Identity.Name;
        var ticket = _mapper.Map<TicketModel>(ticketModel);
        _ticketService.UpdatePrice(id, ticket, login);
        return Accepted();
    }
    

    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult DeleteTicket(int id)
    {
        var login = HttpContext.User.Identity.Name;
        _ticketService.Delete(id, login);
        return NoContent();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTicket(int id)
    {
        _ticketService.Restore(id);
        return NoContent();

    }

}
