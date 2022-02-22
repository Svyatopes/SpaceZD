using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteTransitsController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<RouteTransitOutputModel> GetRouteTransitById(int id)
    {
        return Ok(new RouteTransitModel());
    }

    [HttpGet]
    public ActionResult<List<RouteTransitOutputModel>> GetRouteTransits()
    {
        return Ok(new List<RouteTransitModel> { new RouteTransitModel() });
    }

    [HttpPost]
    public ActionResult AddRouteTransit(RouteTransitInputModel routetransit)
    {
        return StatusCode(StatusCodes.Status201Created, routetransit);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRouteTransit(int id)
    {
        return Accepted();
    }

    [HttpPut("{id}")]
    public ActionResult EditRouteTransit(int id, RouteTransitInputModel routetransit)
    {
        return BadRequest();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreRouteTransit(int id)
    {
        return Accepted();
    }
}