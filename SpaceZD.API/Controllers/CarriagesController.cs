using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
public class CarriagesController : ControllerBase
{
    private readonly ICarriageService _carriageService;
    private readonly IMapper _mapper;
    public CarriagesController(ICarriageService carriageService, IMapper mapper)
    {
        _carriageService = carriageService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<CarriageOutputModel>> GetCarriages(bool  allIncluded)
    {
        var carriagModel = _carriageService.GetList(allIncluded);
        var carriages = _mapper.Map<List<CarriageOutputModel>>(carriagModel);
        if (carriages!=null)
            return Ok(carriages);
        return BadRequest();
    }

    [HttpGet("{id}")]
    public ActionResult<CarriageOutputModel> GetCarriageById(int id)
    {
        var carriagModel = _carriageService.GetById(id);
        var carriage = _mapper.Map<CarriageOutputModel>(carriagModel);
        if (carriage != null)
            return Ok(carriage);
        return BadRequest();
    }

    /*
    TODO на подумать
    [HttpGet("{id}/tickets")]
    public ActionResult<List<TicketOutputModel>> GetGetGetCarriageByIdWithTickets(int id)
    {
        return NotFound("Can't find(((((");
    }*/

    [HttpPost]
    public ActionResult AddCarriage(CarriageInputModel carriage)
    {
        return StatusCode(StatusCodes.Status201Created, carriage);
    }

    [HttpPut("{id}")]
    public ActionResult EditCarriage(int id, CarriageInputModel carriage)
    {
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCarriage(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreCarriage(int id)
    {
        return Accepted();
    }
}