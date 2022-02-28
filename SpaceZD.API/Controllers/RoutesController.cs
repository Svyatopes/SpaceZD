using AutoMapper;
using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
public class RoutesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRouteService _service;

    public RoutesController(IMapper mapper, IRouteService routeService)
    {
        _mapper = mapper;
        _service = routeService;
    }

    //api/Routes
    [HttpGet]
    [SwaggerOperation(Summary = "Get all routes")]
    [ProducesResponseType(typeof(List<RouteOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<RouteOutputModel>> GetRoutes()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetList(userId.Value);
        var result = _mapper.Map<List<RouteOutputModel>>(entities);
        return Ok(result);
    }

    //api/Routes/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted routes")]
    [ProducesResponseType(typeof(List<RouteOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<RouteOutputModel>> GetDeletedRoutes()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<RouteOutputModel>>(entities);
        return Ok(result);
    }

    //api/Routes/42
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get route by id")]
    [ProducesResponseType(typeof(RouteOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<RouteOutputModel> GetRouteById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(userId.Value, id);
        var result = _mapper.Map<RouteOutputModel>(entities);
        return Ok(result);
    }

    //api/Routes
    [HttpPost]
    [SwaggerOperation(Summary = "Adding a new route")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult AddRoute([FromBody] RouteInputModel route)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<RouteModel>(route);
        var idCreate = _service.Add(userId.Value, entity);
        return StatusCode(StatusCodes.Status201Created, idCreate);
    }

    //api/Routes/42
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Editing route by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult EditRoute(int id, [FromBody] RouteInputModel route)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<RouteModel>(route);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/Routes/42
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleting route by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult DeleteRoute(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Delete(userId.Value, id);
        return NoContent();
    }

    //api/Routes/42
    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restoring route by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreRoute(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}