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
public class TrainsController : ControllerBase
{

    private readonly ITrainService _trainService;
    private readonly IMapper _mapper;
    public TrainsController(ITrainService trainService, IMapper mapper)
    {
        _trainService = trainService;
        _mapper = mapper;
    }


    [HttpGet]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all trains")]
    [ProducesResponseType(typeof(List<TrainOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<List<TrainModel>> GetTrains()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var trainModel = _trainService.GetList(userId.Value);
        var trains = _mapper.Map<List<TrainOutputModel>>(trainModel);
        if (trains != null)
            return Ok(trains);
        return BadRequest("Oh.....");
    }


    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted trains")]
    [ProducesResponseType(typeof(List<TrainOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<List<TrainModel>> GetTrainsDelete()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var trainModel = _trainService.GetListDeleted(userId.Value);
        var trains = _mapper.Map<List<TrainOutputModel>>(trainModel);
        if (trains != null)
            return Ok(trains);
        return BadRequest("Oh.....");
    }


    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Get train by id")]
    [ProducesResponseType(typeof(TrainOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<TrainModel> GetTrainById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var trainModel = _trainService.GetById(id, userId.Value);
        var train = _mapper.Map<TrainOutputModel>(trainModel);
        if (train != null)
            return Ok(train);
        else
            return BadRequest("Train doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Add new train")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult AddTrain()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var idAddedEntity = _trainService.Add(new TrainModel(), userId.Value);
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }


    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    [SwaggerOperation(Summary = "Delete train by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult DeleteTrain(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _trainService.Delete(id, userId.Value);
        return NoContent();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restore train by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult RestoreTrain(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _trainService.Restore(id, userId.Value);
        return NoContent();

    }

}
