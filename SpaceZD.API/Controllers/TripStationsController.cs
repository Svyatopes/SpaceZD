using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
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
        return Ok(_mapper.Map<TripStationOutputModel>(_service.GetById(id)));
    }

    //api/TripStations/42
    [HttpPut("{id}")]
    public ActionResult EditTripStation(int id, [FromBody] TripStationUpdateInputModel tripStation)
    {
        _service.Update(id, _mapper.Map<TripStationModel>(tripStation));
        return Accepted();
    }
    
    //api/TripStations/42/set-platform/2
    [HttpPut("{id}/set-platform/{idPlatform}")]
    public ActionResult SetPlatformTripStation(int id, int idPlatform)
    {
        _service.SetPlatform(id, idPlatform);
        return Accepted();
    }

    //api/TripStations/42/ready-platform
    [HttpGet("{id}/ready-platform")]
    public ActionResult<List<PlatformOutputModel>> GetReadyPlatforms(int id)
    {
        return Ok(_mapper.Map<List<PlatformOutputModel>>(_service.GetReadyPlatforms(id)));
    }
}