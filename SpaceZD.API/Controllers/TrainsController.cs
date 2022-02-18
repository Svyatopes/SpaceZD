using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;

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
    public ActionResult<List<TrainModel>> GetTrains()
    {
        var trainModel = _trainService.GetList();
        var trains = _mapper.Map<List<TrainOutputModel>>(trainModel);
        if (trains != null)
            return Ok(trains);
        return BadRequest("Oh.....");
    }



    [HttpGet("{id}")]
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
    public ActionResult AddTrain()
    {
        var idAddedEntity = _trainService.Add(new TrainModel());
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);

    }

    //как будто этого тут не должно быть

    /*
    [HttpPut("{id}")]
    public ActionResult EditTrain(int id, TrainModel train)
    {
        var trainForEdit = _mapper.Map<TrainModel>(train);
        _trainService.Update(id, trainForEdit);
        return Accepted();
    }
    */

    [HttpDelete("{id}")]
    public ActionResult DeleteTrain(int id)
    {
        _trainService.Delete(id);
        return Accepted();
    }


    [HttpPatch("{id}")]
    public ActionResult RestoreTrain(int id)
    {
        _trainService.Restore(id);
        return Accepted();

    }

}
