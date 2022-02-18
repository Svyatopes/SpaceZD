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
public class TripStationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITripStationService _service;

    public TripStationsController(IMapper mapper, ITripStationService service)
    {
        _mapper = mapper;
        _service = service;
    }


    //api/Trips/42
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    [HttpGet("{id}")]
    public ActionResult<TripStationOutputModel> GetTripById(int id)
    {
        return Ok(_mapper.Map<TripStationOutputModel>(_service.GetById(id)));
    }

    //api/Trips/42
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    [HttpPut("{id}")]
    public ActionResult EditTrip(int id, TripStationUpdateInputModel tripStation)
    {
        _service.Update(id, _mapper.Map<TripStationModel>(tripStation));
        return Accepted();
    }

    //api/Trips/42/ready-platform
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    [HttpGet("{id}/ready-platform")]
    public ActionResult<List<PlatformOutputModel>> GetReadyPlatforms(int id)
    {
        return Ok(_mapper.Map<List<PlatformOutputModel>>(_service.GetReadyPlatforms(id)));
    }
}