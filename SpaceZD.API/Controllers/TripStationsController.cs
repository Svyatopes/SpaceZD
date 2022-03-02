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

[Route("api/[controller]")]
[ApiController]
[AuthorizeRole(Role.Admin, Role.StationManager)]
public class TripStationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITripStationService _service;

    public TripStationsController(IMapper mapper, ITripStationService service)
    {
        _mapper = mapper;
        _service = service;
    }


    //api/TripStations/42
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get trip station by id")]
    [ProducesResponseType(typeof(TripStationOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<TripStationOutputModel> GetTripStationById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(userId.Value, id);
        var result = _mapper.Map<TripStationOutputModel>(entities);
        return Ok(result);
    }

    //api/TripStations/42
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Editing trip station by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult EditTripStation(int id, [FromBody] TripStationUpdateInputModel tripStation)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<TripStationModel>(tripStation);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/TripStations/42/set-platform?platformId=22
    [HttpPut("{id}/set-platform")]
    [SwaggerOperation(Summary = "Set platform for trip station id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult SetPlatformTripStation(int id, [FromQuery] int platformId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.SetPlatform(userId.Value, id, platformId);
        return NoContent();
    }

    //api/TripStations/42/ready-platforms
    [HttpGet("{id}/ready-platforms")]
    [SwaggerOperation(Summary = "Get available platform by trip station id")]
    [ProducesResponseType(typeof(List<PlatformOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<PlatformOutputModel>> GetReadyPlatforms(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetReadyPlatforms(userId.Value, id);
        var result = _mapper.Map<List<PlatformOutputModel>>(entities);
        return Ok(result);
    }
}