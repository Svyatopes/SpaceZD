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
public class StationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStationService _stationService;
    public StationsController(IMapper mapper, IStationService stationService)
    {
        _mapper = mapper;
        _stationService = stationService;
    }

    //api/Stations
    [HttpGet]
    public ActionResult<List<StationShortOutputModel>> GetStations()
    {
        var entities = _stationService.GetList();
        var result = _mapper.Map<List<StationShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Stations/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<StationShortOutputModel>> GetDeletedStations()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _stationService.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<StationShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Stations/42
    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult<StationFullOutputModel> GetStationById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _stationService.GetById(userId.Value, id);
        var result = _mapper.Map<StationFullOutputModel>(entities);
        return Ok(result);
    }

    //api/Stations/42/near-stations
    [HttpGet("{id}/near-stations")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult<List<StationShortOutputModel>> GetNearStationsById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _stationService.GetNearStations(userId.Value, id);
        var result = _mapper.Map<List<StationShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Stations
    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult AddStation([FromBody] StationInputModel station)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<StationModel>(station);
        var idCreate = _stationService.Add(userId.Value, entity);
        return StatusCode(StatusCodes.Status201Created, idCreate);
    }

    //api/Stations/42
    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult EditStation(int id, [FromBody] StationInputModel station)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<StationModel>(station);
        _stationService.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/Stations/42
    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult DeleteStation(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _stationService.Delete(userId.Value, id);
        return NoContent();
    }

    //api/Stations/42
    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreStation(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _stationService.Restore(userId.Value, id);
        return NoContent();
    }
}