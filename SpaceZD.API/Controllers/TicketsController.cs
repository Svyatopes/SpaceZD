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

    [HttpGet("id/{orderId}")]
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
    [AuthorizeRole(Role.Admin)]
    public ActionResult<TicketModel> GetTicketById(int id)
    {
        var ticketModel = _ticketService.GetById(id);
        var ticket = _mapper.Map<TicketOutputModel>(ticketModel);
        if (ticket != null)
            return Ok(ticket);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult AddTicket(TicketInputModel ticketModel)
    {
        var ticket = _mapper.Map<TicketModel>(ticketModel);
        var idAddedEntity = _ticketService.Add(ticket);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }

    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult EditTicket(int id, TicketModel ticketModel)
    {
        var ticket = _mapper.Map<TicketModel>(ticketModel);
        _ticketService.Update(id, ticket);
        return Accepted();
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    public ActionResult DeleteTicket(int id)
    {
        _ticketService.Delete(id);
        return Accepted();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTicket(int id)
    {
        _ticketService.Restore(id);
        return Accepted();

    }

}
