using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
public class CarriagesController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<CarriageOutputModel>> GetCarriages()
    {
        return Ok(new List<CarriageOutputModel> { new CarriageOutputModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageOutputModel> GetCarriageById(int id)
    {
        return Ok(new CarriageOutputModel());
    }

    /*
    TODO на подумать
    [HttpGet("{id}/tickets")]
    public ActionResult<List<TicketOutputModel>> GetGetGetCarriageByIdWithTickets(int id)
    {
        return NotFound("Can't find(((((");
    }*/

    [HttpPost]
    public ActionResult AddCarriage(CarriageInputModel carriage)
    {
        return StatusCode(StatusCodes.Status201Created, carriage);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriage(int id, CarriageInputModel carriage)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCarriage(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreCarriage(int id)
    {
        return Accepted();
    }
}