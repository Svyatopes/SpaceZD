using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriageController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<CarriageModel>> GetCarriages()
    {
        return Ok(new List<CarriageModel> { new CarriageModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageModel> GetCarriageById(int id)
    {
        return Ok(new CarriageModel());
    }

    [HttpGet("with-tickets/{id}")]
    public ActionResult<CarriageModel> GetCarriageByIdWithTickets(int id)
    {
        return NotFound("Can't find(((((");
    }

    [HttpPost]
    public ActionResult AddCarriage(CarriageModel carriage)
    {
        return StatusCode(StatusCodes.Status201Created, carriage);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriage(int id, CarriageModel carriage)
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