using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
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

        [HttpPost]
        [Route("assign-monk")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(RetreatGroupViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Assigned monk for retreat groups.")]
        public async Task<ActionResult<RetreatGroupViewModel>> AssignedMonk([FromForm] CreateMonkForGroupModel model)
        {
            return await _retreatGroupService.AssignedMonk(model);
        }
    }
}
