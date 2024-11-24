using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/reactions")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ReactionViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get reaction by id.")]
        public async Task<ActionResult<ReactionViewModel>> GetReaction([FromRoute] Guid id)
        {
            return await _reactionService.GetReaction(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(ReactionViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create reaction.")]
        public async Task<ActionResult<ReactionViewModel>> CreatePost([FromBody] CreateReactionModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var reaction = await _reactionService.CreateReaction(auth!.Id, model);
            return CreatedAtAction(nameof(GetReaction), new { id = reaction.Id }, reaction);
        }



        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Remove reaction.")]
        public async Task<IActionResult> UpdateReaction([FromRoute] Guid id)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var flag = await _reactionService.UpdateReaction(id, auth!.Id);
            if (flag)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
