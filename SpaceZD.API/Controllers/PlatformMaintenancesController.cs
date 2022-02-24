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
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult<List<PlatformMaintenanceOutputModel>> GetPlatformMaintenances()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetList(userId.Value);
        var result = _mapper.Map<List<PlatformMaintenanceOutputModel>>(entities);
        return Ok(result);
    }

    [HttpGet]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<PlatformMaintenanceOutputModel>> GetPlatformMaintenancesDeleted()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var entities = _service.GetListDeleted(userId.Value);
        var result = _mapper.Map<List<PlatformMaintenanceOutputModel>>(entities);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public ActionResult<PlatformMaintenanceOutputModel> GetPlatformMaintenanceById(int id)
    {
        var entities = _service.GetById(id);
        var result = _mapper.Map<PlatformMaintenanceOutputModel>(entities);
        return Ok(result);
       
    }

    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
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
    [AuthorizeRole(Role.Admin, Role.StationManager)]
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
    [AuthorizeRole(Role.Admin, Role.StationManager)]
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