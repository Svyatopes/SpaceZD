using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformMaintenanceController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<PlatformMaintenanceModel>> GetPlatformMaintenances()
    {
        return Ok(new List<PlatformMaintenanceModel> { new PlatformMaintenanceModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<PlatformMaintenanceModel> GetPlatformMaintenanceById(int id)
    {
        return Ok(new PlatformMaintenanceModel());
    }

    [HttpPost]
    public ActionResult AddPlatformMaintenance(PlatformMaintenanceModel platformMaintenance)
    {
        return StatusCode(StatusCodes.Status201Created, platformMaintenance);
    }

    [HttpPut("{id}")]
    public ActionResult EditPlatformMaintenance(int id, PlatformMaintenanceModel platformMaintenance)
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