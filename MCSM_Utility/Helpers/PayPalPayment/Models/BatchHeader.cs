using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class BatchHeader
    {
        [JsonPropertyName("payout_batch_id")]
        public string PayoutBatchId { get; set; } = null!;

        [JsonPropertyName("batch_status")]
        public string BatchStatus { get; set; } = null!;
    }
}
