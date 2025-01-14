﻿using MCSM_Utility.Exceptions;
using MCSM_Utility.Helpers.PayPalPayment.Models;
using MCSM_Utility.Settings;
using System.Net.Http.Headers;
using System.Runtime;
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


        public static async Task<bool> RefundPaymentAsync(string saleId, decimal amount, AppSetting appSettings)
        {
            var accessToken = await GetAccessTokenAsync(appSettings);

            // Yêu cầu hoàn tiền
            var refundRequest = new
            {
                amount = new
                {
                    total = amount.ToString("F2"),
                    currency = "USD"
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(refundRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.PayPal.BaseUrl}/v1/payments/sale/{saleId}/refund");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = requestContent;

            var response = await _httpClient.SendAsync(request);

            // Kiểm tra kết quả trả về
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to refund payment. Error: {errorContent}");
            }

            return true;
        }

        public static async Task<string> GetSaleIdAsync(string paymentId, AppSetting appSettings)
        {
            var accessToken = await GetAccessTokenAsync(appSettings);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{appSettings.PayPal.BaseUrl}/v1/payments/payment/{paymentId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve payment details. Error: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var paymentDetails = JsonSerializer.Deserialize<PayPalPaymentResponse>(json);

            // Lấy sale_id từ transactions
            var saleId = paymentDetails?.Transactions
                ?.SelectMany(t => t.RelatedResources)
                ?.FirstOrDefault(r => r.Sale != null)?.Sale.Id;

            return saleId ?? throw new Exception("Unable to retrieve sale_id from payment details.");
        }

        public static async Task<bool> ExecutePaymentAsync(string paymentId, string payerId, AppSetting appSettings)
        {
            var accessToken = await GetAccessTokenAsync(appSettings);


            // Gọi Execute Payment API
            var request = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.PayPal.BaseUrl}/v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(new { payer_id = payerId }), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Execute Payment Failed: {errorContent}");
            }

            // Nếu thành công
            var result = await response.Content.ReadAsStringAsync();
            var paymentDetails = JsonSerializer.Deserialize<PayPalPaymentResponse>(result);

            return paymentDetails?.State == "approved";
        }


        // Phương thức hoàn tiền qua PayPal
        public static async Task<PayPalPayoutResponse> CreatePayoutAsync(PayPalPayoutModel model, AppSetting appSettings)
        {
            var accessToken = await GetAccessTokenAsync(appSettings);

            var payoutRequest = new
            {
                sender_batch_header = new
                {
                    email_subject = "Refund Request",
                    recipient_type = "EMAIL"
                },
                items = new[]
                    {
                        new
                        {
                            recipient_wallet = "PAYPAL",
                            amount = new
                            {
                                value = model.Amount.ToString("F2"),
                                currency = "USD"
                            },
                            receiver = model.EmailPaypal,
                            note = "Refund for Retreat Registration",
                            sender_item_id = model.ParticipantId.ToString(),
                        }   
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(payoutRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{appSettings.PayPal.BaseUrl}/v1/payments/payouts");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = requestContent;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var errorModel = JsonSerializer.Deserialize<PayoutErrorModel>(errorResponse);
                throw new ConflictException(errorModel!.Message);
                //throw new Exception($"PayPal payout failed. Status code: {response.StatusCode}. Response: {errorResponse}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var payoutResponse = JsonSerializer.Deserialize<PayPalPayoutResponse>(jsonResponse);

            return payoutResponse;
        }

    }
}

