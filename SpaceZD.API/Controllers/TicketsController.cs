using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Configuration;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<TicketModel>> GetTickets()
    {
        return Ok(new List<TicketModel> { new TicketModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<TicketModel> GetTicketById(int id)
    {
        return Ok(new TicketModel());
    }


    [HttpPost]
    public ActionResult AddTicket(TicketInputModel ticket)
    {

        var ticketModel = ApiMapper.GetInstance().Map<TicketModel>(ticket);


        return StatusCode(StatusCodes.Status201Created, ticket);
    }

    [HttpPut("{id}")]
    public ActionResult EditTicket(int id, TicketModel ticket)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTicket(int id)
    {
        return Accepted();
    }

}
