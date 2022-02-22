using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
public class CarriagesController : ControllerBase
{
    private readonly ICarriageService _service;
    private readonly IMapper _mapper;
    public CarriagesController(ICarriageService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<CarriageOutputModel>> GetCarriages()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var carriagModel = _service.GetList(userId.Value);
        var carriages = _mapper.Map<List<CarriageOutputModel>>(carriagModel);
            return Ok(carriages);
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageOutputModel> GetCarriageById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var carriagModel = _service.GetById(userId.Value, id);
        var carriage = _mapper.Map<CarriageOutputModel>(carriagModel);
            return Ok(carriage);
    }

    //api/Trips/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<CarriageOutputModel>> GetDeletedCarriages()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<TripShortOutputModel>>(entities);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult AddCarriage([FromBody] CarriageInputModel carriage)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<CarriageModel>(carriage);
        var idCreate = _service.Add(userId.Value, entity);
        return StatusCode(StatusCodes.Status201Created, idCreate);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriage(int id, [FromBody] CarriageInputModel carriage)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<CarriageModel>(carriage);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCarriage(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
       [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreCarriage(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}