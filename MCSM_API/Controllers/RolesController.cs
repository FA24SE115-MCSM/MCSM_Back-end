using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        //[Authorize(AccountRole.Monks, AccountRole.Practitioners)]
        [ProducesResponseType(typeof(List<RoleViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all roles.")]
        public async Task<ActionResult<List<RoleViewModel>>> GetRoles()
        {
            return await _roleService.GetRoles();
        }
    }
}
