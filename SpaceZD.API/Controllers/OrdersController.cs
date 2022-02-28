using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceZD.API.Attributes;
using SpaceZD.API.Extensions;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;
using System.ComponentModel;

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
    [Description("Get orders of actual authorized user")]
    [ProducesResponseType(typeof(List<OrderShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Get orders of some user by UserId. Only for Admin")]
    [ProducesResponseType(typeof(List<OrderShortOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Get order by Id. Only order that belongs this user or user is in admin role")]
    [ProducesResponseType(typeof(OrderFullOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Adding draft order to authorized user")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Editing draft order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Deleting order by id for authorized user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
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
    [Description("Restoring order by id. Only for admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorOutputModel), StatusCodes.Status404NotFound)]
    public ActionResult RestoreOrder(int id)
    {
        var userId = this.GetUserId();
        if (userId == null)
            return Unauthorized("Not valid token, try login again");

        _orderService.Restore(userId.Value, id);

        return NoContent();
    }
}