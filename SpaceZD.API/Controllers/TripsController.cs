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
    [SwaggerOperation(Summary = "Get all trips")]
    [ProducesResponseType(typeof(List<TripShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<TripShortOutputModel>> GetTrips()
    {
        var entities = _service.GetList();
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }
    
    //api/Trips/from-date?date=2022-02-22
    [HttpGet("from-date")]
    [SwaggerOperation(Summary = "Get all trips from select date")]
    [ProducesResponseType(typeof(List<TripShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<TripShortOutputModel>> GetTripsFromDate([FromQuery] DateTime date)
    {
        var entities = _service.GetList().Where(g => g.StartTime.Date == date.Date);
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }

    //api/Trips/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted trips")]
    [ProducesResponseType(typeof(List<TripShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Get trip by id")]
    [ProducesResponseType(typeof(TripFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<TripFullOutputModel> GetTripById(int id)
    {
        var entities = _service.GetById(id);
        var result = _mapper.Map<TripFullOutputModel>(entities);
        return Ok(result);
    }

    //api/Trips/42/seats
    [HttpPost("{id}/seats")]
    [SwaggerOperation(Summary = "Get seats from trip by id and start/end station id")]
    [ProducesResponseType(typeof(TripFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<CarriageSeatsOutputModel>> GetSeatsByTripId(int id, [FromBody] StartEndIdStationsInputModel model)
    {
        var entities = _service.GetFreeSeat(id, model.StartStationId, model.EndStationId);
        var result = _mapper.Map<List<CarriageSeatsOutputModel>>(entities);
        return Ok(result);
    }

    //api/Trips
    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Adding a new trip")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Editing trip by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Deleting trip by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Restoring trip by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreTrip(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}