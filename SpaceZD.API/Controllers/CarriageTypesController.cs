using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriageTypesController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<CarriageTypeOutputModel>> GetCarriageTypes()
    {
        return Ok(new List<CarriageTypeOutputModel> { new CarriageTypeOutputModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageTypeOutputModel> GetCarriageTypeById(int id)
    {
        return Ok(new CarriageTypeOutputModel());
    }

    [HttpGet("{id}/carriages")]
    public ActionResult<CarriageTypeOutputModel> GetCarriageTypeByIdWithCarriage(int id)
    {
        return Ok(new CarriageTypeOutputModel());
    }

    [HttpPost]
    public ActionResult AddCarriageType(CarriageTypeInputModel carriageType)
    {
        return StatusCode(StatusCodes.Status201Created, carriageType);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriageType(int id, CarriageTypeInputModel carriageType)
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