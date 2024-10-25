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
    [Route("api/tools")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        private readonly IToolService _toolService;

        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<ToolViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all tools.")]
        public async Task<ActionResult<ListViewModel<ToolViewModel>>> GetTools([FromQuery] ToolFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _toolService.GetTools(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ToolViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get tool by id.")]
        public async Task<ActionResult<ToolViewModel>> GetTool([FromRoute] Guid id)
        {
            return await _toolService.GetTool(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ToolViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create tool.")]
        public async Task<ActionResult<ToolViewModel>> CreateRoom([FromForm] CreateToolModel model)
        {
            var tool = await _toolService.CreateTool(model);
            return CreatedAtAction(nameof(GetTool), new { id = tool.Id }, tool);
        }



        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ToolViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update tool.")]
        public async Task<ActionResult<ToolViewModel>> UpdateTool([FromRoute] Guid id, [FromForm] UpdateToolModel model)
        {
            var tool = await _toolService.UpdateTool(id, model);
            return CreatedAtAction(nameof(GetTool), new { id = tool.Id }, tool);
        }
    }
}
