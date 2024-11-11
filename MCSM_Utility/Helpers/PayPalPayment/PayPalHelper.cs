using MCSM_Utility.Helpers.PayPalPayment.Models;
using MCSM_Utility.Settings;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MCSM_Utility.Helpers.PayPalPayment
{
    public static class PayPalHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();


        private static async Task<string> GetAccessTokenAsync(AppSetting appSettings)
        {
            var clientId = appSettings.PayPal.ClientId;
            var clientSecret = appSettings.PayPal.ClientSecret;
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.PayPal.BaseUrl}/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(json);
            return tokenResponse?.AccessToken ?? throw new Exception("Unable to retrieve PayPal access token.");
        }

        public static async Task<PayPalPaymentResponse> CreatePaymentAsync(CreatePayPalModel model, AppSetting appSettings)
        {
            var accessToken = await GetAccessTokenAsync(appSettings);
            model.CancelUrl += $"?paymentId={model.PaymentId}";
            var paymentRequest = new
            {
                intent = "sale",
                redirect_urls = new { return_url = model.ReturnUrl, cancel_url = model.CancelUrl },
                payer = new { payment_method = "paypal" },
                transactions = new[]
                {
                    new
                    {
                        amount = new { total = model.Amount.ToString("F2"), currency = "USD" },
                        description = "Retreat Registration Payment"
                    }
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.PayPal.BaseUrl}/v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = requestContent;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PayPalPaymentResponse>(json);


            return paymentResponse ?? throw new Exception("Unable to retrieve PayPal response.");
        }
    }
}
