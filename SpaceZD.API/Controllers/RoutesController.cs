using AutoMapper;
using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

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
    public ActionResult RestoreRoute(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}