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
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet]
    [AuthorizeRole(Role.User)]
    [ProducesResponseType(typeof(List<OrderShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<OrderShortOutputModel>> GetOrders()
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var orders = _orderService.GetList(userId.Value, userId.Value, false);
        var ordersShortOutputModels = _mapper.Map<List<OrderShortOutputModel>>(orders);

        return Ok(ordersShortOutputModels);
    }

    [HttpGet]
    [Route("by-user/{userId}")]
    [AuthorizeRole(Role.Admin)]
    [ProducesResponseType(typeof(List<OrderShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<OrderShortOutputModel>> GetOrders(int userId)
    {
        var adminId = this.GetUserId();
        if (adminId == null)
            return Unauthorized("Not valid token, try login again");

        var orders = _orderService.GetList(userId, adminId.Value, true);
        var ordersShortOutputModels = _mapper.Map<List<OrderShortOutputModel>>(orders);

        return Ok(ordersShortOutputModels);
    }

    [HttpGet("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    [ProducesResponseType(typeof(OrderFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<OrderFullOutputModel> GetOrderById(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var order = _orderService.GetById(userId.Value, id);
        var orderOutputModel = _mapper.Map<OrderFullOutputModel>(order);
        return Ok(orderOutputModel);
    }

    [HttpPost]
    [AuthorizeRole(Role.User)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult AddOrder([FromBody] OrderAddInputModel order)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        var orderModel = _mapper.Map<OrderModel>(order);
        var createdOrderId = _orderService.Add(userId.Value, orderModel);


        return StatusCode(StatusCodes.Status201Created, createdOrderId);
    }

    [HttpPut("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult EditOrder(int id, [FromBody] OrderEditInputModel order)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");


        var orderModel = _mapper.Map<OrderModel>(order);
        _orderService.Edit(userId.Value, id, orderModel);

        return NoContent();
    }


    [HttpDelete("{id}")]
    [AuthorizeRole(Role.Admin, Role.User)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult DeleteOrder(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _orderService.Delete(userId.Value, id);

        return NoContent();
    }

    [HttpPatch("{id}")]
    [AuthorizeRole(Role.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult RestoreOrder(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _orderService.Restore(userId.Value, id);

        return NoContent();
    }
}