using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<OrderModel>> GetOrders()
    {
        return Ok(new List<OrderModel> { new OrderModel() });
    }

    [HttpGet("{id}")]
    public ActionResult<OrderModel> GetOrderById(int id)
    {
        return Ok(new OrderModel());
    }

    [HttpGet("with-tickets/{id}")]
    public ActionResult<OrderModel> GetOrderByIdWithTickets(int id)
    {
        return NotFound("Can't find(((((");
    }

    [HttpPost]
    public ActionResult AddOrder(OrderModel order)
    {
        return StatusCode(StatusCodes.Status201Created, order);
    }

    [HttpPut("{id}")]
    public ActionResult EditOrder(int id, OrderModel order)
    {
        return BadRequest();
    }

        
    [HttpDelete("{id}")]
    public ActionResult DeleteOrder(int id)
    {
        return Accepted();
    }

    [HttpPatch("{id}")]
    public ActionResult RestoreOrder(int id)
    {
        return Accepted();
    }
}