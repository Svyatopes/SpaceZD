using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<TrainModel>> GetTickets()
    {
        return Ok(new List<TrainModel> { new TrainModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<TrainModel> GetTicketById(int id)
    {
        return Ok(new TrainModel());
    }


    [HttpPost]
    public ActionResult AddTrain(TrainModel train)
    {
        return StatusCode(StatusCodes.Status201Created, train);
    }

    [HttpPut("{id}")]
    public ActionResult EditTrain(int id, TrainModel train)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTrain(int id)
    {
        return Accepted();
    }

}
