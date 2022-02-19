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
[AuthorizeRole(Role.Admin, Role.TrainRouteManager)]
public class CarriageTypesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICarriageTypeService _carriageTypeService;
    public CarriageTypesController(IMapper mapper, ICarriageTypeService carriageTypeService)
    {
        _mapper = mapper;
        _carriageTypeService = carriageTypeService;
    }

    //api/CarriageTypes
    [HttpGet]
    public ActionResult<List<CarriageTypeOutputModel>> GetCarriageTypes()
    {
        return Ok(_mapper.Map<List<CarriageTypeOutputModel>>(_carriageTypeService.GetList()));
    }

    //api/CarriageTypes/deleted
    [HttpGet("deleted")]
    public ActionResult<List<CarriageTypeOutputModel>> GetDeletedCarriageTypes()
    {
        return Ok(_mapper.Map<List<CarriageTypeOutputModel>>(_carriageTypeService.GetListDeleted()));
    }

    //api/CarriageTypes/42
    [HttpGet("{id}")]
    public ActionResult<CarriageTypeOutputModel> GetCarriageTypeById(int id)
    {
        return Ok(_mapper.Map<CarriageTypeOutputModel>(_carriageTypeService.GetById(id)));
    }

    //api/CarriageTypes
    [HttpPost]
    public ActionResult AddCarriageType(CarriageTypeInputModel carriageType)
    {
        _carriageTypeService.Add(_mapper.Map<CarriageTypeModel>(carriageType));
        return StatusCode(StatusCodes.Status201Created);
    }

    //api/CarriageTypes/42
    [HttpPut("{id}")]
    public ActionResult EditCarriageType(int id, CarriageTypeInputModel carriageType)
    {
        _carriageTypeService.Update(id, _mapper.Map<CarriageTypeModel>(carriageType));
        return Accepted();
    }

    //api/CarriageTypes/42
    [HttpDelete("{id}")]
    public ActionResult DeleteCarriageType(int id)
    {
        _carriageTypeService.Delete(id);
        return Accepted();
    }

    //api/CarriageTypes/42
    [HttpPatch("{id}")]
    public ActionResult RestoreCarriageType(int id)
    {
        _carriageTypeService.Restore(id);
        return Accepted();
    }
}