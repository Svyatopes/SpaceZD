using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriagesController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<CarriageFullOutputModel>> GetCarriages()
    {
        return Ok(new List<CarriageFullOutputModel> { new CarriageFullOutputModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageFullOutputModel> GetCarriageById(int id)
    {
        return Ok(new CarriageFullOutputModel());
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