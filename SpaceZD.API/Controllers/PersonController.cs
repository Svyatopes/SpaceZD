using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PersonModel>> GetPersons()
        {
            return Ok(new List<PersonModel> { new PersonModel() });
        }

        [HttpGet("{id}")]
        public ActionResult<PersonModel> GetPersonById(int id)
        {
            return Ok(new PersonModel());
        }

        [HttpGet("with-tickets/{id}")]
        public ActionResult<PersonModel> GetPersonByIdWithTickets(int id)
        {
            return NotFound("Can't find(((((");
        }

        [HttpPost]
        public ActionResult AddPerson(PersonModel Person)
        {
            return StatusCode(StatusCodes.Status201Created, Person);
        }

        [HttpPut("{id}")]
        public ActionResult EditPerson(int id, PersonModel Person)
        {
            return BadRequest();
        }


        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            return Accepted();
        }

        [HttpPatch("{id}")]
        public ActionResult RestorePerson(int id)
        {
            return Accepted();
        }
    }
}
