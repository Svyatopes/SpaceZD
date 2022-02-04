using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<StationModel> GetStationById(int id)
        {
            return Ok(new StationModel());
        }

        [HttpGet]
        public ActionResult<List<StationModel>> GetStations()
        {
            return Ok(new List<StationModel> { new StationModel() });
        }

        [HttpPost]
        public ActionResult AddStation(StationModel station)
        {
            return StatusCode(StatusCodes.Status201Created, station);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStation(int id)
        {
            return Accepted();
        }

        [HttpPut("{id}")]
        public ActionResult EditStation(int id, StationModel station)
        {
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public ActionResult RestoreStation(int id)
        {
            return Accepted();
        }
    }
}
