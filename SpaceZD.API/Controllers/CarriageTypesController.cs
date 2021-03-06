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
public class CarriageTypesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICarriageTypeService _service;
    public CarriageTypesController(IMapper mapper, ICarriageTypeService service)
    {
        _mapper = mapper;
        _service = service;
    }

    //api/CarriageTypes
    [HttpGet]
    public ActionResult<List<CarriageTypeOutputModel>> GetCarriageTypes()
    {
        var entities = _service.GetList();
        var result = _mapper.Map<List<CarriageTypeOutputModel>>(entities);
        return Ok(result);
    }

    //api/CarriageTypes/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<CarriageTypeOutputModel>> GetDeletedCarriageTypes()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<CarriageTypeOutputModel>>(entities);
        return Ok(result);
    }

    //api/CarriageTypes/42
    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult<CarriageTypeOutputModel> GetCarriageTypeById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(userId.Value, id);
        var result = _mapper.Map<CarriageTypeOutputModel>(entities);
        return Ok(result);
    }

    //api/CarriageTypes
    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult AddCarriageType([FromBody] CarriageTypeInputModel carriageType)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<CarriageTypeModel>(carriageType);
        var idCreate = _service.Add(userId.Value, entity);
        return StatusCode(StatusCodes.Status201Created, idCreate);
    }

    //api/CarriageTypes/42
    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult EditCarriageType(int id, [FromBody] CarriageTypeInputModel carriageType)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entity = _mapper.Map<CarriageTypeModel>(carriageType);
        _service.Update(userId.Value, id, entity);
        return NoContent();
    }

    //api/CarriageTypes/42
    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult DeleteCarriageType(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Delete(userId.Value, id);
        return NoContent();
    }

    //api/CarriageTypes/42
    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreCarriageType(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}