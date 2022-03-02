using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get RouteTransit by id")]
    [ProducesResponseType(typeof(List<RouteTransitOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<RouteTransitOutputModel> GetRouteTransitById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var routeTransitModel = _service.GetById(userId.Value, id);
        var result = _mapper.Map<RouteTransitOutputModel>(routeTransitModel);
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all RouteTransit By Routem")]
    [ProducesResponseType(typeof(List<RouteTransitOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<RouteTransitOutputModel>> GetRouteTransitsByRouteId(int routeId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var routeTransitListModel = _service.GetListByRouteId(userId.Value, routeId);
        var result = _mapper.Map<List<RouteTransitOutputModel>>(routeTransitListModel);
        return Ok(result);
    }

    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted RouteTransit By Routem for Admin")]
    [ProducesResponseType(typeof(List<RouteTransitOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<RouteTransitOutputModel>> GetRouteTransitsByRouteIdDeleted(int routeId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var routeTransitListModel = _service.GetListByRouteIdDeleted(userId.Value, routeId);
        var result = _mapper.Map<List<RouteTransitOutputModel>>(routeTransitListModel);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adding a new RouteTransit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult AddRouteTransit([FromBody] RouteTransitInputModel routeTransitInputModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var routeTransitModel = _mapper.Map<RouteTransitModel>(routeTransitInputModel);
        var idRouteTransitCreate = _service.Add(userId.Value, routeTransitModel);
        return StatusCode(StatusCodes.Status201Created, idRouteTransitCreate);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Editing RouteTransit by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult EditRouteTransit(int id, [FromBody] RouteTransitInputModel routeTransitInputModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var routeTransit = _mapper.Map<RouteTransitModel>(routeTransitInputModel);
        _service.Update(userId.Value, id, routeTransit);

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleting RouteTransit by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Restoring RouteTransit by id (only Admin)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreRouteTransit(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return Accepted();
    }
}