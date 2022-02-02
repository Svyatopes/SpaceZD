using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriageTypeController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<CarriageTypeModel>> GetCarriageTypes()
    {
        return Ok(new List<CarriageTypeModel> { new CarriageTypeModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageTypeModel> GetCarriageTypeById(int id)
    {
        return Ok(new CarriageTypeModel());
    }

    [HttpGet("with-carriage/{id}")]
    public ActionResult<CarriageTypeModel> GetCarriageTypeByIdWithCarriage(int id)
    {
        return Ok(new CarriageTypeModel().Carriages);
    }

    [HttpPost]
    public ActionResult AddCarriageType(CarriageTypeModel carriageType)
    {
        return StatusCode(StatusCodes.Status201Created, carriageType);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriageType(int id, CarriageTypeModel carriageType)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCarriageType(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreCarriageType(int id)
    {
        return Accepted();
    }
}