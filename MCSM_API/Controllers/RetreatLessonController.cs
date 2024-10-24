using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/retreats/lessons")]
    [ApiController]
    public class RetreatLessonController : ControllerBase
    {
        private readonly IRetreatLessonService _retreatLessonService;

        public RetreatLessonController(IRetreatLessonService retreatLessonService)
        {
            _retreatLessonService = retreatLessonService;
        }

        [HttpGet]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(ListViewModel<RetreatLessonViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all retreat lessons of a retreat.")]
        public async Task<ActionResult<ListViewModel<RetreatLessonViewModel>>> GetRetreatLessonsOfARetreat(Guid retreatId, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatLessonService.GetRetreatLessonsOfARetreat(retreatId, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatLessonViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat lesson by id.")]
        public async Task<ActionResult<RetreatLessonViewModel>> GetRetreatLesson([FromRoute] Guid id)
        {
            return await _retreatLessonService.GetRetreatLesson(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatLessonViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create retreat lesson.")]
        public async Task<ActionResult<RetreatLessonViewModel>> CreateRetreatLesson([FromBody] CreateRetreatLessonModel model)
        {            
            var rtrLesson = await _retreatLessonService.CreateRetreatLesson(model);
            return CreatedAtAction(nameof(GetRetreatLesson), new { id = rtrLesson.Id }, rtrLesson);
        }



        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove retreat lesson from retreat.")]
        public async Task<IActionResult> DeleteRetreatLesson([FromRoute] Guid id)
        {
            await _retreatLessonService.DeleteRetreatLesson(id);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Retreat lesson đã được dời khỏi retreat!"
            });
        }
    }
}
