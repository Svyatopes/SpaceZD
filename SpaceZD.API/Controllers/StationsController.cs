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
    [SwaggerOperation(Summary = "Get all stations")]
    [ProducesResponseType(typeof(List<StationShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<StationShortOutputModel>> GetStations()
    {
        var entities = _stationService.GetList();
        var result = _mapper.Map<List<StationShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Stations/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted stations")]
    [ProducesResponseType(typeof(List<StationShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Get station by id")]
    [ProducesResponseType(typeof(StationFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Get near stations by station id")]
    [ProducesResponseType(typeof(List<StationShortOutputModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Adding a new station")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Editing station by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Deleting station by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Restoring station by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreStation(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _stationService.Restore(userId.Value, id);
        return NoContent();
    }
}