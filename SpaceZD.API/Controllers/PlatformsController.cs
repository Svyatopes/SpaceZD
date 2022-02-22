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
public class PlatformsController : ControllerBase
{
    private readonly IPlatformService _platformService;
    private readonly IMapper _mapper;

    public PlatformsController(IMapper mapper, IPlatformService platformService)
    {
        _platformService = platformService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("list-by-station/{stationId}")]
    public ActionResult<List<PlatformModel>> GetPlatforms(int stationId)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var platforms = _platformService.GetListByStationId(userId.Value, stationId);
        if (User.IsInRole(Role.Admin.ToString()))
        {
            var platformsList = _mapper.Map<PlatformWithDeletedOutputModel>(platforms);
            return Ok(platformsList);
        }
        else
        {
            var platformsList = _mapper.Map<PlatformOutputModel>(platforms);
            return Ok(platformsList);
        }

    }

    [HttpGet("{id}")]
    public ActionResult<PlatformModel> GetPlatformById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var platform = _platformService.GetById(userId.Value, id);

        return Ok(platform);
    }

    [HttpPost]
    public ActionResult AddPlatform([FromBody] PlatformAddInputModel platformInputModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var platform = _mapper.Map<PlatformModel>(platformInputModel);
        var platformId = _platformService.Add(userId.Value, platform);

        return StatusCode(StatusCodes.Status201Created, platformId);
    }

    [HttpPut("{id}")]
    public ActionResult EditPlatform(int id, [FromBody] PlatformInputModel platformInputModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var platform = _mapper.Map<PlatformModel>(platformInputModel);
        platform.Id = id;

        _platformService.Edit(userId.Value, platform);
        return Ok();
    }


    [HttpDelete("{id}")]
    public ActionResult DeletePlatform(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _platformService.Delete(userId.Value, id);
        return Ok();
    }

    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestorePlatform(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _platformService.Restore(userId.Value, id);
        return Ok();
    }
}