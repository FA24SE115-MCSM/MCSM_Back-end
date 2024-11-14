using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/retreats")]
    [ApiController]
    public class RetreatController : ControllerBase
    {
        private readonly IRetreatService _retreatService;

        public RetreatController(IRetreatService retreatService)
        {
            _retreatService = retreatService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<RetreatViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all retreats.")]
        public async Task<ActionResult<ListViewModel<RetreatViewModel>>> GetRetreats([FromQuery] RetreatFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatService.GetRetreats(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat by id.")]
        public async Task<ActionResult<RetreatViewModel>> GetRetreat([FromRoute] Guid id)
        {
            return await _retreatService.GetRetreat(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(RetreatViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create retreat.")]
        public async Task<ActionResult<RetreatViewModel>> CreateRetreat([FromForm] CreateRetreatModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var retreat = await _retreatService.CreateRetreat(auth!.Id, model);
            return CreatedAtAction(nameof(GetRetreat), new { id = retreat.Id }, retreat);
        }



        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(RetreatViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update retreat.")]
        public async Task<ActionResult<RetreatViewModel>> UpdateAccount([FromRoute] Guid id, [FromForm] UpdateRetreatModel model)
        {
            var retreat = await _retreatService.UpdateRetreat(id, model);
            return CreatedAtAction(nameof(GetRetreat), new { id = retreat.Id }, retreat);
        }

        //-------------------------

        [HttpGet]
        [Route("{retreatId}/progress")]
        [ProducesResponseType(typeof(ProgressTrackingViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get current retreat progress.")]
        public async Task<ProgressTrackingViewModel> GetTrackingProgressOfRetreat([FromRoute] Guid retreatId)
        {
            return await _retreatService.GetTrackingProgressOfRetreat(retreatId);
        }

        [HttpGet]
        [Route("{profileId}/retreats")]
        [ProducesResponseType(typeof(ProgressTrackingViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat history of an account.")]
        public async Task<ListViewModel<RetreatViewModel>> GetRetreatsOfAccount([FromRoute] Guid profileId, [FromQuery] RetreatFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatService.GetRetreatsOfAccount(profileId, filter, pagination);
        }
    }
}
