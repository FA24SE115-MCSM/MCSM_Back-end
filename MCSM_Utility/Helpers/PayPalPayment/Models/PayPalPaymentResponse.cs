using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class PayPalPaymentResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("intent")]
        public string Intent { get; set; } = null!;

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("payer")]
        public Payer Payer { get; set; } = null!;

        [JsonPropertyName("transactions")]
        public List<PayPalTransaction> Transactions { get; set; } = null!;

        [JsonPropertyName("links")]
        public List<PayPalLink> Links { get; set; } = new List<PayPalLink>();

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
    }

    public class PayPalTransaction
    {
        [JsonPropertyName("amount")]
        public PayPalAmount Amount { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;
    }

    public class Payer
    {
        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = null!;
    }

    public class PayPalAmount
    {
        [JsonPropertyName("total")]
        public string Total { get; set; } = null!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = null!;
    }
}
