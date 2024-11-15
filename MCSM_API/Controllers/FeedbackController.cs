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
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ListViewModel<FeedbackViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all feedbacks.")]
        public async Task<ActionResult<ListViewModel<FeedbackViewModel>>> GetFeedbacks([FromQuery] FeedbackFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _feedbackService.GetFeedbacks(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(FeedbackViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get feedback by id.")]
        public async Task<ActionResult<FeedbackViewModel>> GetFeedback([FromRoute] Guid id)
        {
            return await _feedbackService.GetFeedback(id);
        }

        [HttpGet]
        [Route("account/feedbacks")]
        [ProducesResponseType(typeof(FeedbackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get feedback for current user.")]
        public async Task<List<FeedbackViewModel>> GetFeedbackByAccount()
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            return await _feedbackService.GetFeedbackByAccount(auth!.Id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FeedbackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create feedback.")]
        public async Task<ActionResult<FeedbackViewModel>> CreateFeedback([FromForm] CreateFeedbackModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var feedback = await _feedbackService.CreateFeedback(auth!.Id, model);
            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        }

        // PUT api/<FeedbackController>/5
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(FeedbackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update feedback.")]
        public async Task<ActionResult<FeedbackViewModel>> UpdateFeedback([FromRoute] Guid id, [FromForm] UpdateFeedbackModel model)
        {
            var feedback = await _feedbackService.UpdateFeedback(id, model);
            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        }

        // DELETE api/<FeedbackController>/5
        [HttpPut]
        [Authorize(AccountRole.Admin)]
        [Route("{id}/delete")]
        [ProducesResponseType(typeof(FeedbackViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Soft delete feedback.")]
        public async Task<ActionResult<FeedbackViewModel>> SoftDeleteFeedback([FromRoute] Guid id)
        {
            var feedback = await _feedbackService.SoftDeleteFeedback(id);
            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        }
    }
}
