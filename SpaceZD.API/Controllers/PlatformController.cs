using Microsoft.AspNetCore.Mvc;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PlatformModel>> GetPlatforms()
        {
            return Ok(new List<PlatformModel> { new PlatformModel() });
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformModel> GetPlatformById(int id)
        {
            return Ok(new PlatformModel());
        }

        [HttpPost]
        public ActionResult AddPlatform(PlatformModel Platform)
        {
            return StatusCode(StatusCodes.Status201Created, Platform);
        }

        [HttpPut("{id}")]
        public ActionResult EditPlatform(int id, PlatformModel Platform)
        {
            return BadRequest();
        }


        [HttpDelete("{id}")]
        public ActionResult DeletePlatform(int id)
        {
            return Accepted();
        }

        [HttpPatch("{id}")]
        public ActionResult RestorePlatform(int id)
        {
            return Accepted();
        }
    }
}
