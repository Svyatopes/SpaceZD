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
    public ActionResult RestorePlatformMaintenance(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _service.Restore(userId.Value, id);
        return Accepted();
    }
}