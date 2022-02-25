using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.StationManager)]
public class RouteTransitsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRouteTransitService _service;

    public RouteTransitsController(IMapper mapper, IRouteTransitService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet("{id}")]
    public ActionResult<RouteTransitOutputModel> GetRouteTransitById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(userId.Value, id);
        var result = _mapper.Map<RouteTransitOutputModel>(entities);
        return Ok(result);
    }

    [HttpGet]
    public ActionResult<List<RouteTransitOutputModel>> GetRouteTransits()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetList(userId.Value);
        var result = _mapper.Map<List<RouteTransitOutputModel>>(entities);
        return Ok(result);
    }

    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<RouteTransitOutputModel>> GetRouteTransitsDeleted()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<RouteTransitOutputModel>>(entities);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult AddRouteTransit([FromBody] RouteTransitInputModel routetransit)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _mapper.Map<RouteTransitModel>(routetransit);
        var idRouteTransitCreate = _service.Add(userId.Value, entities);
        return StatusCode(StatusCodes.Status201Created, idRouteTransitCreate);
    }

    [HttpPut("{id}")]
    public ActionResult EditRouteTransit(int id, [FromBody] RouteTransitInputModel routetransit)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _mapper.Map<RouteTransitModel>(routetransit);
        _service.Update(userId.Value, id, entities);

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRouteTransit(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Delete(userId.Value, id);
        return Accepted();
    }

    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreRouteTransit(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return Accepted();
    }
}