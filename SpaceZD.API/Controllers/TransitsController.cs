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
public class TransitsController : ControllerBase
{

    private readonly ITransitService _transitService;
    private readonly IMapper _mapper;
    public TransitsController(ITransitService transitService, IMapper mapper)
    {
        _transitService = transitService;
        _mapper = mapper;
    }


    [HttpGet]
    [AuthorizeRole(Role.Admin)]
    public ActionResult<List<TransitOutputModel>> GetTransits()
    {
        var transitModel = _transitService.GetList();
        var transits = _mapper.Map<List<TransitOutputModel>>(transitModel);
        if (transits != null)
            return Ok(transits);
        return BadRequest();
    }


    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult<TransitOutputModel> GetTransitById(int id)
    {
        var transitModel = _transitService.GetById(id);
        var transit = _mapper.Map<TransitOutputModel>(transitModel);
        if (transit != null)
            return Ok(transit);
        else
            return BadRequest("Transit doesn't exist");
    }


    [HttpPost]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult AddTransit(TransitCreateInputModel transitModel)
    {
        var transit = _mapper.Map<TransitModel>(transitModel);
        var idAddedEntity = _transitService.Add(transit);
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult EditTransit(int id, TransitUpdateInputModel transit)
    {

        var transitForEdit = _mapper.Map<TransitModel>(transit);
        _transitService.Update(id, transitForEdit);
        return Accepted();

    }


    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.StationManager)]
    public ActionResult DeleteTransit(int id)
    {
        _transitService.Delete(id);
        return Accepted();

    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    public ActionResult RestoreTransit(int id)
    {
        _transitService.Restore(id);
        return Accepted();

    }

}
