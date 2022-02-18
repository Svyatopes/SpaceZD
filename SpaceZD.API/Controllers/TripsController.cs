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
public class TripsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITripService _service;

    public TripsController(IMapper mapper, ITripService service)
    {
        _mapper = mapper;
        _service = service;
    }

    //api/Trips
    [HttpGet]
    public ActionResult<List<TripShortOutputModel>> GetTrips()
    {
        return Ok(_mapper.Map<List<TripShortOutputModel>>(_service.GetList()));
    }
    
    //api/Trips/2022-1-1
    [HttpGet("{date}")]
    public ActionResult<List<TripShortOutputModel>> GetTripsFromDate(DateTime date)
    {
        return Ok(_mapper.Map<List<TripShortOutputModel>>(_service.GetList().Where(g => g.StartTime.Date == date.Date).ToList()));
    }

    //api/Trips/deleted
    [AuthorizeRole(Role.Admin)]
    [HttpGet("deleted")]
    public ActionResult<List<TripShortOutputModel>> GetDeletedTrips()
    {
        return Ok(_mapper.Map<List<TripShortOutputModel>>(_service.GetListDeleted()));
    }

    //api/Trips/42
    [HttpGet("{id}")]
    public ActionResult<TripFullOutputModel> GetTripById(int id)
    {
        return Ok(_mapper.Map<TripFullOutputModel>(_service.GetById(id)));
    }

    //api/Trips/42/free-seats
    [HttpGet("{id}/free-seats")]
    public ActionResult<List<CarriageSeatsOutputModel>> GetFreeSeatsByTripId(int id, StartEndIdStationsInputModel model)
    {
        var freeSeats = _service.GetFreeSeat(id, model.StartStationId, model.EndStationId);
        foreach (var csm in freeSeats)
        {
            csm.Seats = csm.Seats.Where(g => g.IsFree).ToList();
        }
        return Ok(_mapper.Map<List<CarriageSeatsOutputModel>>(freeSeats));
    }

    //api/Trips/42/seats
    [AuthorizeRole(Role.Admin)]
    [HttpGet("{id}/seats")]
    public ActionResult<List<CarriageSeatsOutputModel>> GetSeatsByTripId(int id, StartEndIdStationsInputModel model)
    {
        return Ok(_mapper.Map<List<CarriageSeatsOutputModel>>(_service.GetFreeSeat(id, model.StartStationId, model.EndStationId)));
    }

    //api/Trips
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [HttpPost]
    public ActionResult AddTrip(TripCreateInputModel trip)
    {
        _service.Add(_mapper.Map<TripModel>(trip));
        return StatusCode(StatusCodes.Status201Created);
    }

    //api/Trips/42
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [HttpPut("{id}")]
    public ActionResult EditTrip(int id, TripUpdateInputModel trip)
    {
        _service.Update(id, _mapper.Map<TripModel>(trip));
        return Accepted();
    }

    //api/Trips/42
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [HttpDelete("{id}")]
    public ActionResult DeleteTrip(int id)
    {
        _service.Delete(id);
        return Accepted();
    }

    //api/Trips/42
    [AuthorizeRole(Role.Admin)]
    [HttpPatch("{id}")]
    public ActionResult RestoreTrip(int id)
    {
        _service.Restore(id);
        return Accepted();
    }
}