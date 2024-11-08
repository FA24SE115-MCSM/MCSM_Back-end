using System.Text.Json.Serialization;

namespace MCSM_Utility.Helpers.PayPalPayment
{
    public class PayPalTokenResponse
    {

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("app_id")]
        public string AppId { get; set; } = null!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

    }
}
