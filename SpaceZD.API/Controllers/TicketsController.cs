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
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticketModel = _ticketService.GetList(userId.Value);
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest("Oh.....");
    }


    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<TicketModel>> GetTicketsDelete()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticketModel = _ticketService.GetListDeleted(userId.Value);
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest();
    }



    [HttpGet("{id}")]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult<TicketModel> GetTicketById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticketModel = _ticketService.GetById(id, userId.Value);
        var ticket = _mapper.Map<TicketOutputModel>(ticketModel);
        return Ok(ticket);
    }


    [HttpPost]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult AddTicket([FromBody] TicketCreateInputModel ticketModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticket = _mapper.Map<TicketModel>(ticketModel);
        var idAddedEntity = _ticketService.Add(userId.Value, ticket);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }



    [HttpDelete("{id}")]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult DeleteTicket(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _ticketService.Delete(id, userId.Value);
        return NoContent();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTicket(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _ticketService.Restore(id, userId.Value);
        return NoContent();

    }

}
