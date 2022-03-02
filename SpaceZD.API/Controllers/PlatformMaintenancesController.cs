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
[AuthorizeRole(Role.Admin, Role.StationManager)]
public class PlatformMaintenancesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPlatformMaintenanceService _service;

    public PlatformMaintenancesController(IMapper mapper, IPlatformMaintenanceService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    [Route("list-by-station/{stationId}")]
    [SwaggerOperation(Summary = "Get all PlatformMaintenances For Station")]
    [ProducesResponseType(typeof(List<PlatformMaintenanceOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<PlatformMaintenanceOutputModel>> GetPlatformMaintenances(int stationId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListByStationId(stationId, userId.Value);
        var result = _mapper.Map<List<PlatformMaintenanceOutputModel>>(entities);
        return Ok(result);
    }

    [HttpGet]
    [Route("list-by-station-deleted/{stationId}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted PlatformMaintenances For Station for Admin")]
    [ProducesResponseType(typeof(List<PlatformMaintenanceOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<List<PlatformMaintenanceOutputModel>> GetPlatformMaintenancesDeleted(int stationId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeletedByStationId(stationId, userId.Value);
        var result = _mapper.Map<List<PlatformMaintenanceOutputModel>>(entities);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get PlatformMaintenance by id")]
    [ProducesResponseType(typeof(List<PlatformMaintenanceOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult<PlatformMaintenanceOutputModel> GetPlatformMaintenanceById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetById(id, userId.Value);
        var result = _mapper.Map<PlatformMaintenanceOutputModel>(entities);
        return Ok(result);
       
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adding a new PlatformMaintenance")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult AddPlatformMaintenance([FromBody] PlatformMaintenanceInputModel platformMaintenance)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _mapper.Map<PlatformMaintenanceModel>(platformMaintenance);
        var idPlatformMaintenanceCreate = _service.Add(userId.Value, entities);
        return StatusCode(StatusCodes.Status201Created, idPlatformMaintenanceCreate);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Editing PlatformMaintenance by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult EditPlatformMaintenance(int id, [FromBody] PlatformMaintenanceInputModel platformMaintenance)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _mapper.Map<PlatformMaintenanceModel>(platformMaintenance);
        _service.Update(userId.Value, id, entities);

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleting PlatformMaintenance by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult DeletePlatformMaintenance(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Delete(userId.Value, id);
        return Accepted();
    }

    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restoring carriage by id (only Admin)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestorePlatformMaintenance(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return Accepted();
    }
}