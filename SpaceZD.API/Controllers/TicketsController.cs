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
    [SwaggerOperation(Summary = "Get all tickets")]
    [ProducesResponseType(typeof(List<TicketOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get all deleted tickets")]
    [ProducesResponseType(typeof(List<TicketOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get tickets by order id")]
    [ProducesResponseType(typeof(List<TicketOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Get ticket by id")]
    [ProducesResponseType(typeof(TicketOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<TicketModel> GetTicketById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var ticketModel = _ticketService.GetById(id, userId.Value);
        var ticket = _mapper.Map<TicketOutputModel>(ticketModel);
        if (ticket != null)
            return Ok(ticket);
        return BadRequest("User doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.User, Role.Admin)]
    [SwaggerOperation(Summary = "Add new ticket")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Delete ticket by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

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
    [SwaggerOperation(Summary = "Restore ticket by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult RestoreTicket(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _ticketService.Restore(id, userId.Value);
        return NoContent();

    }

}
