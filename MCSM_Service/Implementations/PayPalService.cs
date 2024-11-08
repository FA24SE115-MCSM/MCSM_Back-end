using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Exceptions;
using MCSM_Utility.Helpers.PayPalPayment;
using MCSM_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCSM_Service.Implementations
{
    public class PayPalService : BaseService, IPayPalService
    {
        private readonly AppSetting _appSettings;
        private readonly IPaymentRepository _paymentRepository;
        private readonly HttpClient _httpClient;
        public PayPalService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings, HttpClient httpClient) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
            _paymentRepository = unitOfWork.Payment;
        }



        public async Task<List<PaymentViewModel>> GetPayments()
        {
            return await _paymentRepository.GetAll()
                .ProjectTo<PaymentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var clientId = _appSettings.PayPal.ClientId;
            var clientSecret = _appSettings.PayPal.ClientSecret;
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_appSettings.PayPal.BaseUrl}/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(json);
            return tokenResponse?.AccessToken ?? throw new Exception("Unable to retrieve PayPal access token.");
        }

        public async Task<string> CreatePaymentAsync(decimal amount, string returnUrl, string cancelUrl, Guid retreatRegistrationId)
        {
            var accessToken = await GetAccessTokenAsync();

            var paymentId = GeneratePaymentId();

            returnUrl = returnUrl + $"/{paymentId}";
            var paymentRequest = new
            {
                intent = "sale",
                redirect_urls = new { return_url = returnUrl, cancel_url = cancelUrl },
                payer = new { payment_method = "paypal" },
                transactions = new[]
                {
                    new
                    {
                        amount = new { total = amount.ToString("F2"), currency = "USD" },
                        description = "Retreat Registration Payment"
                    }
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_appSettings.PayPal.BaseUrl}/v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = requestContent;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PayPalPaymentResponse>(json);

            var payment = new Payment
            {
                Id = paymentId,
                RetreatRegId = retreatRegistrationId,
                PaymentMethod = "PayPal",
                PaypalOrderId = paymentResponse.Id,
                Description = $"Retreat Registration Payment: {amount.ToString("F2")}",
                Status = "Pending"
            };

            _paymentRepository.Add(payment);
            await _unitOfWork.SaveChanges();

            return paymentResponse?.Links?.Find(link => link.Rel == "approval_url")?.Href
                ?? throw new Exception("Unable to retrieve PayPal approval URL.");
        }


        public async Task<PaymentViewModel> UpdatePaymentStatus(string paymentId)
        {
            var payment = await _paymentRepository.GetMany(p => p.Id == paymentId).Include(p => p.RetreatReg).FirstOrDefaultAsync() ?? throw new NotFoundException("Payment not found");

            payment.Status = "Success";
            payment.RetreatReg.IsPaid = true;

            _paymentRepository.Update(payment);
            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                var returnPayment = _mapper.Map<PaymentViewModel>(payment);
                return returnPayment;
            }
            return null!;
        }

        private static string GeneratePaymentId()
        {
            long ticks = DateTime.UtcNow.Ticks;
            int hash = HashCode.Combine(ticks);
            uint positiveHash = (uint)hash & 0x7FFFFFFF;
            string hashString = positiveHash.ToString("X8");
            string id = "PAY" + hashString;

            return id;
        }
    }
}
