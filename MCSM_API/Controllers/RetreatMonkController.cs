using FirebaseAdmin.Messaging;
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
    [Route("api/retreats/monks")]
    [ApiController]
    public class RetreatMonkController : ControllerBase
    {
        private readonly IRetreatMonkService _retreatMonkService;

        public RetreatMonkController(IRetreatMonkService retreatMonkService)
        {
            _retreatMonkService = retreatMonkService;
        }

        [HttpGet]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(ListViewModel<RetreatMonkViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all retreat monks of a retreat.")]
        public async Task<ActionResult<ListViewModel<RetreatMonkViewModel>>> GetRetreatMonks(Guid retreatId, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatMonkService.GetRetreatMonksOfARetreat(retreatId, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatMonkViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat monk by id.")]
        public async Task<ActionResult<RetreatMonkViewModel>> GetRetreatMonk([FromRoute] Guid id)
        {
            return await _retreatMonkService.GetRetreatMonk(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatMonkViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create retreat monk.")]
        public async Task<ActionResult<RetreatMonkViewModel>> CreateRetreatMonk([FromBody] CreateRetreatMonkModel model)
        {            
            var monk = await _retreatMonkService.CreateRetreatMonk(model);
            return CreatedAtAction(nameof(GetRetreatMonk), new { id = monk.Id }, monk);
        }



        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove retreat monk from retreat.")]
        public async Task<IActionResult> DeleteRetreatMonk([FromRoute] Guid id)
        {
            await _retreatMonkService.DeleteRetreatMonk(id);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Retreat monk đã được dời khỏi retreat!"
            });            
        }
    }
}
