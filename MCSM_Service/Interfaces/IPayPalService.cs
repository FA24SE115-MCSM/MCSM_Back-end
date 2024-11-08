using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IPayPalService
    {
        Task<List<PaymentViewModel>> GetPayments();
        Task<string> CreatePaymentAsync(decimal amount, string returnUrl, string cancelUrl, Guid retreatRegistrationId);
        Task<PaymentViewModel> UpdatePaymentStatus(string paymentId);
    }
}
