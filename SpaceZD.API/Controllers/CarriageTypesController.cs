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
    [SwaggerOperation(Summary = "Get сarriage types depending on the role of an authorized user",
        Description = "Admin and TrainRouteManager role get CarriageTypeFullOutputModel other roles CarriageTypeShortOutputModel")]
    [ProducesResponseType(typeof(List<CarriageTypeShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<CarriageTypeFullOutputModel>> GetCarriageTypes()
    {
        var entities = _service.GetList();
        if (User.IsInRole(Role.Admin.ToString()) || User.IsInRole(Role.TrainRouteManager.ToString()))
        {
            var resultFull = _mapper.Map<List<CarriageTypeFullOutputModel>>(entities);
            return Ok(resultFull);
        }
        var resultShort = _mapper.Map<List<CarriageTypeShortOutputModel>>(entities);
        return Ok(resultShort);
    }

    //api/CarriageTypes/deleted
    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted сarriage types (only Admin)")]
    [ProducesResponseType(typeof(List<CarriageTypeFullOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<CarriageTypeFullOutputModel>> GetDeletedCarriageTypes()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<CarriageTypeFullOutputModel>>(entities);
        return Ok(result);
    }

    //api/CarriageTypes/42
    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Get сarriage type by id")]
    [ProducesResponseType(typeof(List<CarriageTypeFullOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<CarriageTypeFullOutputModel> GetCarriageTypeById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(userId.Value, id);
        var result = _mapper.Map<CarriageTypeFullOutputModel>(entities);
        return Ok(result);
    }

    //api/CarriageTypes
    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Adding a new сarriage type")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Editing carriage type by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Deleting carriage type by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [SwaggerOperation(Summary = "Restoring carriage type by id (only Admin)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreCarriageType(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return NoContent();
    }
}