using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetOrders()
        {
            return Ok("Must be orders");
        }

        [HttpGet("{id}")]
        public ActionResult GetOrderById(int id)
        {
            return Ok("MustBeOrder");
        }

        [HttpGet("with-tickets/{id}")]
        public ActionResult GetOrderByIdWithTickets(int id)
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
}
