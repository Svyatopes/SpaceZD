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
    [HttpGet("deleted")]
    public ActionResult<List<StationShortOutputModel>> GetDeletedStations()
    {
        return Ok(_mapper.Map<List<StationShortOutputModel>>(_stationService.GetListDeleted()));
    }

    //api/Stations/42
    [HttpGet("{id}")]
    public ActionResult<StationFullOutputModel> GetStationById(int id)
    {
        return Ok(_mapper.Map<StationFullOutputModel>(_stationService.GetById(id)));
    }

    //api/Stations/42/near-stations
    [HttpGet("{id}/near-stations")]
    public ActionResult<List<StationShortOutputModel>> GetNearStationsById(int id)
    {
        return Ok(_mapper.Map<List<StationShortOutputModel>>(_stationService.GetNearStations(id)));
    }

    //api/Stations
    [HttpPost]
    public ActionResult AddStation(StationInputModel station)
    {
        _stationService.Add(_mapper.Map<StationModel>(station));
        return StatusCode(StatusCodes.Status201Created);
    }

    //api/Stations/42
    [HttpPut("{id}")]
    public ActionResult EditStation(int id, StationInputModel station)
    {
        _stationService.Update(id, _mapper.Map<StationModel>(station));
        return Accepted();
    }

    //api/Stations/42
    [HttpDelete("{id}")]
    public ActionResult DeleteStation(int id)
    {
        _stationService.Delete(id);
        return Accepted();
    }

    //api/Stations/42
    [HttpPatch("{id}")]
    public ActionResult RestoreStation(int id)
    {
        _stationService.Restore(id);
        return Accepted();
    }
}