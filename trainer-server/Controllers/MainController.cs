using trainer.server.Helpers;
using Microsoft.AspNetCore.Mvc;
using trainer.server.Infrastructure.Managers.Trainer;

namespace trainer.server.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : ControllerBase
    {
        [HttpGet]
        [Route("get/exercises")]
        public async Task<IActionResult> ExercisesByCategory([FromQuery] string category)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new TrainerManager().ExercisesByCategory(category);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
