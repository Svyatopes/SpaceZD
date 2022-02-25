using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.StationManager)]
public class PlatformMaintenancesController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<PlatformMaintenanceOutputModel>> GetPlatformMaintenances()
    {
        return Ok(new List<PlatformMaintenanceOutputModel> { new PlatformMaintenanceOutputModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<PlatformMaintenanceOutputModel> GetPlatformMaintenanceById(int id)
    {
        return Ok(new PlatformMaintenanceOutputModel());
    }

    [HttpPost]
    public ActionResult AddPlatformMaintenance(PlatformMaintenanceInputModel platformMaintenance)
    {
        return StatusCode(StatusCodes.Status201Created, platformMaintenance);
    }

    [HttpPut("{id}")]
    public ActionResult EditPlatformMaintenance(int id, PlatformMaintenanceInputModel platformMaintenance)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePlatformMaintenance(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestorePlatformMaintenance(int id)
    {
        return Accepted();
    }
}