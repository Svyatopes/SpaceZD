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
        var entities = _service.GetList();
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }
    
    //api/Trips/from-date/2022-1-1
    [HttpGet("from-date/{date}")]
    public ActionResult<List<TripShortOutputModel>> GetTripsFromDate(DateTime date)
    {
        var entities = _service.GetList().Where(g => g.StartTime.Date == date.Date);
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Trips/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<TripShortOutputModel>> GetDeletedTrips()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Trips/42
    [HttpGet("{id}")]
    public ActionResult<TripFullOutputModel> GetTripById(int id)
    {
        var entities = _service.GetById(id);
        var result = _mapper.Map<TripFullOutputModel>(entities);
        return Ok(result);
    }

    //api/Trips/42/seats
    [HttpPost("{id}/seats")]
    public ActionResult<List<CarriageSeatsOutputModel>> GetSeatsByTripId(int id, [FromBody] StartEndIdStationsInputModel model)
    {
        var entities = _service.GetFreeSeat(id, model.StartStationId, model.EndStationId);
        var result = _mapper.Map<List<CarriageSeatsOutputModel>>(entities);
        return Ok(result);
    }

    //api/Trips
    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult AddTrip([FromBody] TripCreateInputModel trip)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<TripModel>(trip);
        var idCreate = _service.Add(userId.Value, entity);
        return StatusCode(StatusCodes.Status201Created, idCreate);
    }

    //api/Trips/42
    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult EditTrip(int id, [FromBody] TripUpdateInputModel trip)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<TripModel>(trip);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/Trips/42
    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult DeleteTrip(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Delete(userId.Value, id);
        return NoContent();
    }

    //api/Trips/42
    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTrip(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}