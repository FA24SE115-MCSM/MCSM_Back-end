using MCSM_Data;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
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
        private readonly IPayPalService _payPalService;
        private readonly IRetreatRegistrationService _retreatRegistrationService;


        public PaymentController(IPayPalService payPalService, IRetreatRegistrationService retreatRegistrationService)
        {
            _payPalService = payPalService;
            _retreatRegistrationService = retreatRegistrationService;

        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PaymentViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all payments.")]
        public async Task<ActionResult<List<PaymentViewModel>>> GetRoles()
        {
            return await _payPalService.GetPayments();
        }

        [HttpPost]
        public async Task<IActionResult> Pay([FromBody]Guid retreatRegistrationId)
        {
            var retreatRegistration = await _retreatRegistrationService.GetRetreatRegistration(retreatRegistrationId);
            if (retreatRegistration == null) return NotFound();
            if (retreatRegistration.IsPaid)
            {
                return Conflict("Retreat registration already paid");
            }

            var amount = retreatRegistration.TotalCost;
            var returnUrl = "http://127.0.0.1:5173/payment";
            var cancelUrl = "https://www.youtube.com";

            var approvalUrl = await _payPalService.CreatePaymentAsync(amount, returnUrl, cancelUrl, retreatRegistrationId);

            return Ok(approvalUrl);
        }

        [HttpPost]
        [Route("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromBody] string paymentId)
        {
            var payment = await _payPalService.UpdatePaymentStatus(paymentId);
            
            return Ok(payment);
        }

        [HttpGet]
        [Route("payment-cancel")]
        public IActionResult PaymentCancel([FromBody]Guid id)
        {

            return Ok("Payment canceled.");
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {

            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            dynamic webhookEvent = JsonConvert.DeserializeObject(body);
            string eventType = webhookEvent.event_type;

            if (eventType == "PAYMENT.SALE.COMPLETED")
            {
                var saleId = (string)webhookEvent.resource.id;
                var retreatRegId = (Guid)webhookEvent.resource.custom_id; 

                Console.WriteLine($"retreatRegId: {retreatRegId}");

            }

            return Ok();
        }

    }
}
