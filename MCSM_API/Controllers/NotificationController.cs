using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        //private readonly IDeviceTokenRepository _deviceTokenRepository;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet]
        [Authorize(AccountRole.Admin, AccountRole.Nun, AccountRole.Monk, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(ListViewModel<NotificationViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all notifications of logged in account.")]
        public async Task<ActionResult<ListViewModel<NotificationViewModel>>> GetNotifications([FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            return await _notificationService.GetNotifications(auth!.Id, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(NotificationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get notification by id.")]
        public async Task<ActionResult<NotificationViewModel>> GetNotification([FromRoute] Guid Id)
        {
            return await _notificationService.GetNotification(Id);
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(NotificationViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Update notification.")]
        public async Task<IActionResult> UpdateNotification([FromRoute] Guid Id, [FromBody] UpdateNotificationModel model)
        {
            var notification = await _notificationService.UpdateNotification(Id, model);
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        [HttpPut]
        [Route("mark-as-read/{accountId}")]
        [ProducesResponseType(typeof(NotificationViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Mark as read all notification.")]
        public async Task<IActionResult> MarkAsReadNotification([FromRoute] Guid accountId)
        {
            var notification = await _notificationService.MakeAsRead(accountId);
            return Ok(notification);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Test send notification.")]
        public async Task<ActionResult<string>> SendNotification([FromBody] Guid accountId)
        {
            var check = await _notificationService.TestSendNotification(accountId);

            if (check)
            {
                return Ok(new
                {
                    message = "Send success",
                    timestamp = DateTime.UtcNow
                });
            }
            else
                return BadRequest(new
                {
                    message = "Send failed",
                });
            }
        }
    
}
