using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<RouteModel> GetRouteById(int id)
        {
            return Ok(new RouteModel());
        }

        [HttpGet]
        public ActionResult<List<RouteModel>> GetRoutes()
        {
            return Ok(new List<RouteModel> { new RouteModel() });
        }

        [HttpPost]
        public ActionResult AddRoute(RouteModel route)
        {
            return StatusCode(StatusCodes.Status201Created, route);
        }

        [HttpDelete]
        public ActionResult DeleteRoute(int id)
        {
            return Accepted();
        }

        [HttpPut("{id}")]
        public ActionResult EditRoute(int id, RouteModel route)
        {
            return BadRequest();
        }
    }
}
