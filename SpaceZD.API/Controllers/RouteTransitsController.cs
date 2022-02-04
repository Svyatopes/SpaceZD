using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteTransitsController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<RouteTransitModel> GetRouteTransitById(int id)
        {
            return Ok(new RouteTransitModel());
        }

        [HttpGet]
        public ActionResult<List<RouteTransitModel>> GetRouteTransits()
        {
            return Ok(new List<RouteTransitModel> { new RouteTransitModel() });
        }

        [HttpPost]
        public ActionResult AddRouteTransit(RouteTransitModel routetransit)
        {
            return StatusCode(StatusCodes.Status201Created, routetransit);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRouteTransit(int id)
        {
            return Accepted();
        }

        [HttpPut("{id}")]
        public ActionResult EditRouteTransit(int id, RouteTransitModel routetransit)
        {
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public ActionResult RestoreRouteTransit(int id)
        {
            return Accepted();
        }
    }
}
