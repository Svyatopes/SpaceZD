using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationsController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<StationFullOutputModel> GetStationById(int id)
    {
        return Ok(new StationModel());
    }

    [HttpGet]
    public ActionResult<List<StationShortOutputModel>> GetStations()
    {
        return Ok(new List<StationShortOutputModel> { new StationShortOutputModel() });
    }

    [HttpPost]
    public ActionResult AddStation(StationInputModel station)
    {
        return StatusCode(StatusCodes.Status201Created, station);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteStation(int id)
    {
        return Accepted();
    }

    [HttpPut("{id}")]
    public ActionResult EditStation(int id, StationInputModel station)
    {
        return BadRequest();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreStation(int id)
    {
        return Accepted();
    }
}