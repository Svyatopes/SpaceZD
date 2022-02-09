using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<RouteFullOutputModel> GetRouteById(int id)
        {
            return Ok(new RouteModel());
        }

        [HttpGet]
        public ActionResult<List<RouteShortOutputModel>> GetRoutes()
        {
            return Ok(new List<RouteShortOutputModel> { new RouteShortOutputModel() });
        }

        [HttpPost]
        public ActionResult AddRoute(RouteInsertInputModel route)
        {
            return StatusCode(StatusCodes.Status201Created, route);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteRoute(int id)
        {
            return Accepted();
        }

        [HttpPut("{id}")]
        public ActionResult EditRoute(int id, RouteUpdateInputModel route)
        {
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public ActionResult RestoreRoute(int id)
        {
            return Accepted();
        }
    }
}
