using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCSM_API.Controllers
{
    [Route("api/group/schedule")]
    [ApiController]
    public class GroupScheduleController : ControllerBase
    {
        private readonly IGroupScheduleService _groupScheduleService;
        public GroupScheduleController(IGroupScheduleService groupScheduleService)
        {
            _groupScheduleService = groupScheduleService;
        }
        // GET: api/<GroupScheduleController>
        [HttpGet]
        //[Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ListViewModel<GroupScheduleViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all group schedules.")]
        public async Task<ActionResult<ListViewModel<GroupScheduleViewModel>>> GetGroupSchedules([FromQuery] GroupScheduleFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _groupScheduleService.GetGroupSchedules(filter, pagination);
        }

        // GET api/<GroupScheduleController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GroupScheduleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get group schedule by id.")]
        public async Task<ActionResult<GroupScheduleViewModel>> GetGroupSchedule([FromRoute] Guid id)
        {
            return await _groupScheduleService.GetGroupSchedule(id);
        }

        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(GroupScheduleViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create group schedule.")]
        public async Task<ActionResult<GroupScheduleViewModel>> CreateDish([FromForm] CreateGroupScheduleModel model)
        {
            var dish = await _groupScheduleService.CreateGroupSchedule(model);
            return CreatedAtAction(nameof(GetGroupSchedule), new { id = dish.Id }, dish);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(GroupScheduleViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update group schedule.")]
        public async Task<ActionResult<GroupScheduleViewModel>> UpdateFeedback([FromRoute] Guid id, [FromForm] UpdateGroupScheduleModel model)
        {
            var dish = await _groupScheduleService.UpdateGroupSchedule(id, model);
            return CreatedAtAction(nameof(GetGroupSchedule), new { id = dish.Id }, dish);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove group schedule.")]
        public async Task<IActionResult> DeleteGroupSchedule([FromRoute] Guid id)
        {
            await _groupScheduleService.DeleteGroupSchedule(id);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Lịch nhóm đã được xóa."
            });
        }
    }
}
