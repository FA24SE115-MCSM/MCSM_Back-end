using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Post;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/device-tokens")]
    [ApiController]
    public class DeviceTokenController : ControllerBase
    {
        private readonly IDeviceTokenService _deviceTokenService;

        public DeviceTokenController(IDeviceTokenService deviceTokenService)
        {
            _deviceTokenService = deviceTokenService;
        }

        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Nun, AccountRole.Monk, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Create a new device token for accounts to use for notifications.")]
        public async Task<ActionResult<bool>> CreateDeviceToken([FromBody] CreateDeviceTokenModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            return await _deviceTokenService.CreateDeviceToken(auth!.Id, model);
        }
    }
}
