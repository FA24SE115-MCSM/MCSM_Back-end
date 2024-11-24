using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/retreat-groups")]
    [ApiController]
    public class RetreatGroupController : ControllerBase
    {
        private readonly IRetreatGroupService _retreatGroupService;

        public RetreatGroupController(IRetreatGroupService retreatGroupService)
        {
            _retreatGroupService = retreatGroupService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<RetreatGroupViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all retreat groups.")]
        public async Task<ActionResult<ListViewModel<RetreatGroupViewModel>>> GetAccounts([FromQuery] RetreatGroupFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatGroupService.GetRetreatGroups(filter, pagination);
        }
    }
}
