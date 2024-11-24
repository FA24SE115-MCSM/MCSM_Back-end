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
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get comment by id.")]
        public async Task<ActionResult<CommentViewModel>> GetComment([FromRoute] Guid id)
        {
            return await _commentService.GetComment(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create comment.")]
        public async Task<ActionResult<CommentViewModel>> CreateComment([FromBody] CreateCommentModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var comment = await _commentService.CreateComment(auth!.Id, model);
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }



        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Update comment.")]
        public async Task<ActionResult<CommentViewModel>> UpdateComment([FromRoute] Guid id, [FromBody] UpdateCommentModel model)
        {
            var comment = await _commentService.UpdateComment(id, model);
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }
    }
}
