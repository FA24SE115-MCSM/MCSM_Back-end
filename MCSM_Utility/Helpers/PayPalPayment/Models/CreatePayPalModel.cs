namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class CreatePayPalModel
    {
        public string PaymentId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; } = null!;
        public string CancelUrl { get; set; } = null!;

    }
}
