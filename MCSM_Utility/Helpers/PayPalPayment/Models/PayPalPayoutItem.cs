using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class PayPalPayoutItem
    {
        [JsonPropertyName("recipient")]
        public string Recipient { get; set; } = null!;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("note")]
        public string Note { get; set; } = null!;
    }
}
