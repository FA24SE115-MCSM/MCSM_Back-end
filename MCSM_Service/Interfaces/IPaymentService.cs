using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Utility.Enums;

namespace MCSM_Service.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentViewModel>> GetPayments(PaymentFilterModel filter);
        Task<PayPalReturnModel> CreatePayment(Guid retreatRegId);
        Task<PaymentViewModel> UpdatePaymentStatus(PayPalPaymentReturn model, PaymentStatus status);
        Task<PaymentViewModel> PayPalPaymentCancel(PayPalCancelModel model);
        Task<RefundViewModel> RefundPayment(Guid accountId, CreateRefundModel model);
        Task UpdateRefund(string refundId);

        //----------------------------------------------------------------
        Task<ListViewModel<PaymentViewModel>> ViewCustomerPaymentHistory(Guid customerId, PaginationRequestModel pagination);
    }
}
