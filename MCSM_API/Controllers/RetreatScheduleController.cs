using MCSM_API.Configurations.Middleware;
using MCSM_Data.Entities;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCSM_API.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class RetreatScheduleController : ControllerBase
    {
        private readonly IRetreatScheduleService _retreatScheduleService;
        public RetreatScheduleController(IRetreatScheduleService retreatScheduleService)
        {
            _retreatScheduleService = retreatScheduleService;
        }
        // GET: api/<RetreatScheduleController>
        [HttpGet("retreat/{retreatId}")]
        [ProducesResponseType(typeof(ListViewModel<RetreatScheduleViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get all retreat schedules of a course.")]
        public async Task<ActionResult<ListViewModel<RetreatScheduleViewModel>>> GetRetreatSchedulesOfARetreat(Guid retreatId, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatScheduleService.GetRetreatSchedulesOfARetreat(retreatId, pagination);
        }

        //GET api/<RetreatScheduleController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatLessonViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat lesson by id.")]
        public async Task<ActionResult<RetreatScheduleViewModel>> GetRetreatSchedule([FromRoute] Guid id)
        {
            return await _retreatScheduleService.GetRetreatSchedule(id);
        }

        // POST api/<RetreatScheduleController>
        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatScheduleViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create retreat schedule.")]
        public async Task<ActionResult<RetreatLessonViewModel>> CreateRetreatSchedule([FromBody] CreateRetreatScheduleModel model)
        {
            var rtrSchedule = await _retreatScheduleService.CreateRetreatSchedule(model);
            return CreatedAtAction(nameof(GetRetreatSchedule), new { id = rtrSchedule.Id }, rtrSchedule);
        }

        // PUT api/<RetreatScheduleController>/5
        [HttpPut("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatScheduleViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update retreat schedule lesson and time.")]
        public async Task<ActionResult<RetreatScheduleViewModel>> UpdateRetreatSchedule([FromRoute] Guid id, [FromBody] UpdateRetreatScheduleModel model)
        {
            var schedule = await _retreatScheduleService.UpdateRetreatSchedule(id, model);
            return CreatedAtAction(nameof(GetRetreatSchedule), new { id = schedule.Id }, schedule);
        }

        // DELETE api/<RetreatScheduleController>/5
        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove retreat schedule.")]
        public async Task<IActionResult> DeleteRetreatSchedule([FromRoute] Guid id)
        {
            await _retreatScheduleService.DeleteRetreatSchedule(id);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Retreat schedule đã bị xóa!"
            });
        }
    }
}
