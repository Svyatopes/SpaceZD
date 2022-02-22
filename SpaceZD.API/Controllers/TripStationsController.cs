using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

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
    public ActionResult EditTripStation(int id, [FromBody] TripStationUpdateInputModel tripStation)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<TripStationModel>(tripStation);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/TripStations/42/set-platform/2
    [HttpPut("{id}/set-platform/{idPlatform}")]
    public ActionResult SetPlatformTripStation(int id, int idPlatform)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.SetPlatform(userId.Value, id, idPlatform);
        return NoContent();
    }

    //api/TripStations/42/ready-platforms
    [HttpGet("{id}/ready-platforms")]
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