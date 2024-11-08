using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment
{
    public class PayPalLink
    {
        [JsonPropertyName("href")]

        public string? Href { get; set; }

        [JsonPropertyName("rel")]

        public string? Rel { get; set; }
    }
}
