using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment.Models
{
    public class PayoutErrorModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!; 
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;
    }
}
