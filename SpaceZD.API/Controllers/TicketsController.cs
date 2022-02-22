using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using SpaceZD.API.Extensions;

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
    
    
    [HttpGet("delete")]
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

    [HttpGet("by-order/{orderId}")]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult<List<TicketModel>> GetTicketByOrderId(int orderId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticketModel = _ticketService.GetListByOrderId(orderId, userId.Value);
        var tickets = _mapper.Map<List<TicketOutputModel>>(ticketModel);
        if (tickets != null)
            return Ok(tickets);
        return BadRequest("Oh.....");
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
        if (ticket != null)
            return Ok(ticket);
        else
            return BadRequest("User doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult AddTicket([FromBody]TicketCreateInputModel ticketModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticket = _mapper.Map<TicketModel>(ticketModel);
        var idAddedEntity = _ticketService.Add(ticket, userId.Value);

        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }

    [HttpPut("{id}")]
    [AuthorizeRole(Role.User, Role.Admin)]
    public ActionResult EditTicket(int id, TicketUpdateInputModel ticketModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticket = _mapper.Map<TicketModel>(ticketModel);
        _ticketService.Update(id, ticket, userId.Value);
        return Accepted();
    }


    [HttpPut("price/{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult EditPriceTicket(int id, TicketUpdatePriceInputModel ticketModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticket = _mapper.Map<TicketModel>(ticketModel);
        _ticketService.UpdatePrice(id, ticket, userId.Value);
        return Accepted();
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
