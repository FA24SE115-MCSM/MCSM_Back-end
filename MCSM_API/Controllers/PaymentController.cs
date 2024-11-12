using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payPalService;
        private readonly IRetreatRegistrationService _retreatRegistrationService;


        public PaymentController(IPaymentService payPalService, IRetreatRegistrationService retreatRegistrationService)
        {
            _payPalService = payPalService;
            _retreatRegistrationService = retreatRegistrationService;

        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PaymentViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all payments.")]
        public async Task<ActionResult<List<PaymentViewModel>>> GetRoles([FromQuery] PaymentFilterModel filter)
        {
            return await _payPalService.GetPayments(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PayPalReturnModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create PayPal payment.")]
        public async Task<ActionResult<PayPalReturnModel>> Pay([FromBody] CreatePayPalPaymentModel model)
        {
            var approvalUrl = await _payPalService.CreatePayment(model.RetreatRegId);
            return CreatedAtAction(nameof(Pay), new { id = model.RetreatRegId }, approvalUrl);
        }

        [HttpPost]
        [Route("payment-success")]
        [ProducesResponseType(typeof(PayPalReturnModel), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Return PayPal payment success.")]
        public async Task<ActionResult<PaymentViewModel>> PaymentSuccess([FromBody] PayPalPaymentReturn model)
        {
            var payment = await _payPalService.UpdatePaymentStatus(model, PaymentStatus.Success);

            return CreatedAtAction(nameof(PaymentSuccess), new { id = model.PayPalPaymentId }, payment);
        }

        [HttpPost]
        [Route("payment-cancel")]
        [SwaggerOperation(Summary = "Return PayPal payment cancel.")]
        public async Task<ActionResult<PaymentViewModel>> PaymentCancel([FromBody] PayPalCancelModel model)
        {
            var payment = await _payPalService.PayPalPaymentCancel(model);
            return CreatedAtAction(nameof(PaymentCancel), new { id = model.PaymentId }, payment);

        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            dynamic webhookEvent = JsonConvert.DeserializeObject(body)!;
            string eventType = webhookEvent.event_type;

            if (eventType == "PAYMENT.SALE.COMPLETED")
            {
                var saleId = (string)webhookEvent.resource.id;
                var retreatRegId = (Guid)webhookEvent.resource.custom_id; 

                Console.WriteLine($"retreatRegId: {retreatRegId}");

            }

            return Ok();
        }

        //------------------------------------------------------------

        [HttpGet]
        [Route("{profileId}/payment")]
        [ProducesResponseType(typeof(PaymentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get payment history of an account.")]
        public async Task<ListViewModel<PaymentViewModel>> ViewCustomerPaymentHistory([FromRoute] Guid profileId, [FromQuery] PaginationRequestModel pagination)
        {
            return await _payPalService.ViewCustomerPaymentHistory(profileId, pagination);
        }
    }
}
