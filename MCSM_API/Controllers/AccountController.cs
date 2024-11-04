using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace MCSM_API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<AccountViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all accounts.")]
        public async Task<ActionResult<ListViewModel<AccountViewModel>>> GetAccounts([FromQuery] AccountFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _accountService.GetAccounts(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(AccountViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get account by id.")]
        public async Task<ActionResult<AccountViewModel>> GetAccount([FromRoute] Guid id)
        {
            return await _accountService.GetAccount(id);
        }

        [HttpGet]
        [Route("verification/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Verify account.")]
        public async Task<IActionResult> VerifyAccount([FromRoute] string token)
        {
            await _accountService.VerifyAccount(token);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Tài khoản đã được xác thực thành công!"
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create account.")]
        public async Task<ActionResult<AccountViewModel>> CreateAccount([FromBody] CreateAccountModel model)
        {
            var account = await _accountService.CreateAccount(model);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(AccountViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update account.")]
        public async Task<ActionResult<AccountViewModel>> UpdateAccount([FromRoute] Guid id, [FromBody] UpdateAccountModel model)
        {
            var account = await _accountService.UpdateAccount(id, model);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPut]
        [Route("avatar")]
        [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
        [ProducesResponseType(typeof(AccountViewModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Upload avatar for account.")]
        public async Task<ActionResult<AccountViewModel>> UploadAvatar([Required] IFormFile image)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var account = await _accountService.UploadAvatar(auth!.Id, image);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPut]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Reset password.")]
        public async Task<ActionResult<AccountViewModel>> ResetPassword([FromBody] ResetPasswordModel model)
        {
            await _accountService.ResetPassword(model);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Password has been reset and sent to your email!"
            });
        }
    }
}
