using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class PayPalPayoutResponse
    {
        [JsonPropertyName("batch_header")]
        public BatchHeader BatchHeader { get; set; } = null!;

        [JsonPropertyName("items")]
        public List<PayPalPayoutItem> Items { get; set; } = new List<PayPalPayoutItem>();
    }
}
