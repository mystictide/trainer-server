using Microsoft.AspNetCore.Mvc;

namespace trainer.server.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : ControllerBase
    {
        [HttpGet]
        [Route("get/artist")]
        public async Task<IActionResult> GetArtist([FromQuery] int ID)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
