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
    [Route("api/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(ListViewModel<LessonViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all lessons.")]
        public async Task<ActionResult<ListViewModel<LessonViewModel>>> GetLessons([FromQuery] LessonFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _lessonService.GetLessons(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LessonViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get lesson by id.")]
        public async Task<ActionResult<LessonViewModel>> GetLesson([FromRoute] Guid id)
        {
            return await _lessonService.GetLesson(id);
        }



        [HttpPost]        
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(LessonViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create lesson.")]
        public async Task<ActionResult<RetreatViewModel>> CreateLesson([FromBody] CreateLessonModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var lesson = await _lessonService.CreateLesson(auth!.Id, model);
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }



        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update lesson.")]
        public async Task<ActionResult<RetreatViewModel>> UpdateLesson([FromRoute] Guid id, [FromBody] UpdateLessonModel model)
        {
            var lesson = await _lessonService.UpdateLesson(id, model);
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }
    }
}
