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
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<PostViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all posts.")]
        public async Task<ActionResult<ListViewModel<PostViewModel>>> GetPosts([FromQuery] PostFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _postService.GetPosts(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get post by id.")]
        public async Task<ActionResult<PostViewModel>> GetPost([FromRoute] Guid id)
        {
            return await _postService.GetPost(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create post.")]
        public async Task<ActionResult<PostViewModel>> CreatePost([FromForm] CreatePostModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var post = await _postService.CreatePost(auth!.Id, model);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }



        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Update post.")]
        public async Task<ActionResult<PostViewModel>> UpdateAccount([FromRoute] Guid id, [FromForm] UpdatePostModel model)
        {
            var post = await _postService.UpdatePost(id, model);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

    }
}
