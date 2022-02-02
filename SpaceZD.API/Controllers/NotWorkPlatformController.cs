using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotWorkPlatformController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<NotWorkPlatformModel>> GetNotWorkPlatforms()
    {
        return Ok(new List<NotWorkPlatformModel> { new NotWorkPlatformModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<NotWorkPlatformModel> GetNotWorkPlatformById(int id)
    {
        return Ok(new NotWorkPlatformModel());
    }

    [HttpPost]
    public ActionResult AddNotWorkPlatform(NotWorkPlatformModel notWorkPlatform)
    {
        return StatusCode(StatusCodes.Status201Created, notWorkPlatform);
    }

    [HttpPut("{id}")]
    public ActionResult EditNotWorkPlatform(int id, NotWorkPlatformModel notWorkPlatform)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteNotWorkPlatform(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreNotWorkPlatform(int id)
    {
        return Accepted();
    }
}