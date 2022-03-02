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
[AuthorizeRole(Role.Admin, Role.StationManager)]

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
    [SwaggerOperation(Summary = "Get all transits")]
    [ProducesResponseType(typeof(List<TransitOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<List<TransitOutputModel>> GetTransits()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var transitModel = _transitService.GetList(userId.Value);
        var transits = _mapper.Map<List<TransitOutputModel>>(transitModel);
        if (transits != null)
            return Ok(transits);
        return BadRequest();
    }


    [HttpGet("deleted")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Get all deleted transits (only Admin)")]
    [ProducesResponseType(typeof(List<TransitOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<List<TransitOutputModel>> GetTransitsDelete()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var transitModel = _transitService.GetListDeleted(userId.Value);
        var transits = _mapper.Map<List<TransitOutputModel>>(transitModel);
        if (transits != null)
            return Ok(transits);
        return BadRequest();
    }


    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get transit by id")]
    [ProducesResponseType(typeof(TransitOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult<TransitOutputModel> GetTransitById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var transitModel = _transitService.GetById(id, userId.Value);
        var transit = _mapper.Map<TransitOutputModel>(transitModel);
        if (transit != null)
            return Ok(transit);
        return BadRequest("Transit doesn't exist");
    }


    [HttpPost]
    [SwaggerOperation(Summary = "Add new transit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult AddTransit([FromBody] TransitCreateInputModel transitModel)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var transit = _mapper.Map<TransitModel>(transitModel);
        var idAddedEntity = _transitService.Add(transit, userId.Value);
        return StatusCode(StatusCodes.Status201Created, idAddedEntity);
    }


    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Edit transit by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult EditTransit(int id, [FromBody] TransitUpdateInputModel transit)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var transitForEdit = _mapper.Map<TransitModel>(transit);
        _transitService.Update(id, transitForEdit, userId.Value);
        return NoContent();

    }


    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete transit by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult DeleteTransit(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _transitService.Delete(id, userId.Value);
        return NoContent();

    }


    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [SwaggerOperation(Summary = "Restore transit by id (only Admin)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]

    public ActionResult RestoreTransit(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _transitService.Restore(id, userId.Value);
        return NoContent();

    }

}
