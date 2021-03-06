using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

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
    public ActionResult<List<TrainModel>> GetTrains()
    {
        var trainModel = _trainService.GetList();
        var trains = _mapper.Map<List<TrainOutputModel>>(trainModel);
        if (trains != null)
            return Ok(trains);
        return BadRequest("Oh.....");
    }



    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult<TrainModel> GetTrainById(int id)
    {
        var trainModel = _trainService.GetById(id);
        var train = _mapper.Map<TrainOutputModel>(trainModel);
        if (train != null)
            return Ok(train);
        else
            return BadRequest("Train doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult AddTrain()
    {
        var idAddedEntity = _trainService.Add(new TrainModel());
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }


    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
    public ActionResult DeleteTrain(int id)
    {
        _trainService.Delete(id);
        return Accepted();
    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTrain(int id)
    {
        _trainService.Restore(id);
        return Accepted();

    }

}
