using AutoMapper;
using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
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
    private readonly IRouteService _routeService;
    public RoutesController(IMapper mapper, IRouteService routeService)
    {
        _mapper = mapper;
        _routeService = routeService;
    }

    //api/Routes
    [HttpGet]
    public ActionResult<List<RouteOutputModel>> GetRoutes()
    {
        return Ok(_mapper.Map<List<RouteOutputModel>>(_routeService.GetList()));
    }

    //api/Routes/deleted
    [HttpGet("deleted")]
    public ActionResult<List<RouteOutputModel>> GetDeletedRoutes()
    {
        return Ok(_mapper.Map<List<RouteOutputModel>>(_routeService.GetListDeleted()));
    }

    //api/Routes/42
    [HttpGet("{id}")]
    public ActionResult<RouteOutputModel> GetRouteById(int id)
    {
        return Ok(_mapper.Map<RouteOutputModel>(_routeService.GetById(id)));
    }

    //api/Routes
    [HttpPost]
    public ActionResult AddRoute(RouteInputModel route)
    {
        _routeService.Add(_mapper.Map<RouteModel>(route));
        return StatusCode(StatusCodes.Status201Created);
    }

    //api/Routes/42
    [HttpPut("{id}")]
    public ActionResult EditRoute(int id, RouteInputModel route)
    {
        _routeService.Update(id, _mapper.Map<RouteModel>(route));
        return Accepted();
    }

    //api/Routes/42
    [HttpDelete("{id}")]
    public ActionResult DeleteRoute(int id)
    {
        _routeService.Delete(id);
        return Accepted();
    }

    //api/Routes/42
    [HttpPatch("{id}")]
    public ActionResult RestoreRoute(int id)
    {
        _routeService.Restore(id);
        return Accepted();
    }
}