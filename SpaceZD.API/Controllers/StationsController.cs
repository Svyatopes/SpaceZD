using AutoMapper;
using SpaceZD.API.Models;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;

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
        return Ok(_mapper.Map<List<StationShortOutputModel>>(_stationService.GetList()));
    }

    //api/Stations/deleted
    [HttpGet("/deleted")]
    public ActionResult<List<StationShortOutputModel>> GetStationsDelete()
    {
        return Ok(_mapper.Map<List<StationShortOutputModel>>(_stationService.GetListDeleted()));
    }

    //api/Stations/42
    [HttpGet("{id:int}")]
    public ActionResult<StationFullOutputModel> GetStationById(int id)
    {
        return Ok(_mapper.Map<StationFullOutputModel>(_stationService.GetById(id)));
    }
    
    //api/Stations/42/near-stations
    [HttpGet("{id:int}/near-stations")]
    public ActionResult<List<StationShortOutputModel>> GetNearStationsById(int id)
    {
        return Ok(_mapper.Map<List<StationShortOutputModel>>(_stationService.GetNearStations(id)));
    }
    
    //api/Stations/42/work-platforms
    [HttpGet("{id:int}/ready-platforms")]
    public ActionResult<List<PlatformOutputModel>> GetReadyPlatformsStationById(int id)
    {
        return Ok(_mapper.Map<List<PlatformOutputModel>>(_stationService.GetReadyPlatformsStationById(id, DateTime.Now)));
    }
    
    //api/Stations/42/work-platforms/2022-10-05
    [HttpGet("{id:int}/ready-platforms/{date:DateTime}")]
    public ActionResult<List<PlatformOutputModel>> GetReadyPlatformsStationById(int id, DateTime date)
    {
        return Ok(_mapper.Map<List<PlatformOutputModel>>(_stationService.GetReadyPlatformsStationById(id, date)));
    }

    //api/Stations
    [HttpPost]
    public ActionResult AddStation(StationInputModel station)
    {
        _stationService.Add(_mapper.Map<StationModel>(station));
        return StatusCode(StatusCodes.Status201Created);
    }

    //api/Stations/42
    [HttpPut("{id:int}")]
    public ActionResult EditStation(int id, StationInputModel station)
    {
        _stationService.Update(id, _mapper.Map<StationModel>(station));
        return Accepted();
    }

    //api/Stations/42
    [HttpDelete("{id:int}")]
    public ActionResult DeleteStation(int id)
    {
        _stationService.Delete(id);
        return Accepted();
    }

    //api/Stations/42
    [HttpPatch("{id:int}")]
    public ActionResult RestoreStation(int id)
    {
        _stationService.Restore(id);
        return Accepted();
    }
}