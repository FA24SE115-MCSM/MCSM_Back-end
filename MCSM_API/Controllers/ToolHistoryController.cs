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
    [Route("api/tool-histories")]
    [ApiController]
    public class ToolHistoryController : ControllerBase
    {
        private readonly IToolHistoryService _toolHistoryService;

        public ToolHistoryController(IToolHistoryService toolHistoryService)
        {
            _toolHistoryService = toolHistoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<ToolHistoryViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all tool histories.")]
        public async Task<ActionResult<ListViewModel<ToolHistoryViewModel>>> GetTools([FromQuery] ToolHistoryFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _toolHistoryService.GetToolHistories(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ToolHistoryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get tool history by id.")]
        public async Task<ActionResult<ToolHistoryViewModel>> GetToolHistory([FromRoute] Guid id)
        {
            return await _toolHistoryService.GetToolHistory(id);
        }



        [HttpPost]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ToolHistoryViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create tool history.")]
        public async Task<ActionResult<List<ToolHistoryViewModel>>> CreateToolHistory([FromBody] CreateToolHistoryModel model)
        {
            var toolHistory = await _toolHistoryService.CreateToolHistory(model);
            return CreatedAtAction(nameof(GetToolHistory), new { id = toolHistory }, toolHistory);
        }



        //[HttpPut]
        //[Route("{id}")]
        //[Authorize(AccountRole.Admin)]
        //[ProducesResponseType(typeof(ToolHistoryViewModel), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        //[SwaggerOperation(Summary = "Update tool history.")]
        //public async Task<ActionResult<ToolHistoryViewModel>> UpdateToolHistory([FromRoute] Guid id, [FromBody] UpdateToolHistoryModel model)
        //{
        //    var toolHistory = await _toolHistoryService.UpdateToolHistory(id, model);
        //    return CreatedAtAction(nameof(GetToolHistory), new { id = toolHistory.Id }, toolHistory);
        //}
    }
}
