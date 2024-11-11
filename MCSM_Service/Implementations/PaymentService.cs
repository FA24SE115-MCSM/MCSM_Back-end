using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using MCSM_Utility.Helpers.PayPalPayment;
using MCSM_Utility.Helpers.PayPalPayment.Models;
using MCSM_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MCSM_Service.Implementations
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly AppSetting _appSettings;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _paymentRepository = unitOfWork.Payment;
        }



        public async Task<List<PaymentViewModel>> GetPayments(PaymentFilterModel filter)
        {
            var query = _paymentRepository.GetAll();

            if (filter.AccountId.HasValue)
            {
                query = query.Where(p => p.AccountId == filter.AccountId.Value);
            }

            if (filter.RetreatRegistrationId.HasValue)
            {
                query = query.Where(p => p.RetreatRegId == filter.RetreatRegistrationId.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(p => p.Status == filter.Status.Value.ToString());
            }


            return await query
                .OrderByDescending(p => p.CreateAt)
                .ProjectTo<PaymentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


        public async Task<PayPalReturnModel> CreatePayment(Guid retreatRegId)
        {
            var retreatReg = await _retreatRegistrationRepository.GetMany(reg => reg.Id == retreatRegId).FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat registration not found");
            if (retreatReg.IsPaid)
            {
                throw new ConflictException("Retreat registration has been paid");
            }
            if (retreatReg.IsDeleted)
            {
                throw new ConflictException("Retreat registration has been delete");
            }

            var paymentId = GeneratePaymentId();
            var createPaymentModel = new CreatePayPalModel
            {
                PaymentId = paymentId,
                Amount = retreatReg.TotalCost,
                ReturnUrl = _appSettings.PayPal.ReturnUrl,
                CancelUrl = _appSettings.PayPal.CancelUrl,
            };
            var paymentResponse = await PayPalHelper.CreatePaymentAsync(createPaymentModel, _appSettings);

            var payment = new Payment
            {
                Id = paymentId,
                AccountId = retreatReg.CreateBy,
                RetreatRegId = retreatRegId,
                PaymentMethod = "PayPal",
                PaypalPaymentId = paymentResponse.Id,
                Amount = retreatReg.TotalCost,
                Description = $"Retreat Registration Payment",
                Status = PaymentStatus.Pending.ToString()
            };

            _paymentRepository.Add(payment);
            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                var approvalUrl = paymentResponse?.Links?.Find(link => link.Rel == "approval_url")?.Href
                ?? throw new Exception("Unable to retrieve PayPal approval URL.");
                var returnResult = new PayPalReturnModel
                {
                    PaymentUrl = approvalUrl
                };
                return returnResult;
            }
            return null!;
        }


        public async Task<PaymentViewModel> UpdatePaymentStatus(PayPalPaymentReturn model, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetMany(p => p.PaypalPaymentId == model.PayPalPaymentId).Include(p => p.RetreatReg).FirstOrDefaultAsync() ?? throw new NotFoundException("Payment not found");

            payment.Status = status.ToString();
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

        public async Task<PaymentViewModel> PayPalPaymentCancel(PayPalCancelModel model)
        {
            var payment = await _paymentRepository.GetMany(p => p.Id == model.PaymentId).Include(p => p.RetreatReg).ThenInclude(retreat => retreat.Retreat).FirstOrDefaultAsync() ?? throw new NotFoundException("Payment not found");

            payment.Status = PaymentStatus.Cancel.ToString();

            _paymentRepository.Update(payment);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
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
