using trainer.server.Helpers;
using Microsoft.AspNetCore.Mvc;
using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;
using trainer.server.Infrastructure.Managers.Trainer;

namespace trainer.server.Controllers
{
    [ApiController]
    [Route("cms")]
    public class CMSController : ControllerBase
    {
        [HttpPost]
        [Route("manage/category")]
        public async Task<IActionResult> ManageCategory([FromBody] Category model)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new TrainerManager().ManageCategories(model);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("manage/exercise")]
        public async Task<IActionResult> ManageExercise([FromBody] Exercise model)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new TrainerManager().ManageExercises(model);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("filter/exercises")]
        public async Task<IActionResult> FilterExercises([FromBody] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new TrainerManager().FilterExercises(filter);
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext))
                {
                    var result = await new TrainerManager().GetCategories();
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
