namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class PayPalPayoutModel
    {
        public Guid ParticipantId { get; set; }
        public string EmailPaypal { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
