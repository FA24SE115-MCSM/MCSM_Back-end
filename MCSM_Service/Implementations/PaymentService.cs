using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using MCSM_Utility.Helpers.PayPalPayment;
using MCSM_Utility.Helpers.PayPalPayment.Models;
using MCSM_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities;

namespace MCSM_Service.Implementations
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly AppSetting _appSettings;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IRetreatRegistrationParticipantRepository _retreatRegistrationParticipantRepository;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _paymentRepository = unitOfWork.Payment;
            _refundRepository = unitOfWork.Refund;
            _retreatRegistrationParticipantRepository = unitOfWork.RetreatRegistrationParticipant;
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
            if (!string.IsNullOrEmpty(filter.CustomerEmail))
            {
                query = query.Where(p => p.Account.Email.Contains(filter.CustomerEmail));
            }
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                query = query.Where(p => $"{p.Account.Profile.FirstName} {p.Account.Profile.LastName}".Contains(filter.CustomerEmail));
            }


            return await query
                .Where(q => q.Status != PaymentStatus.Pending.ToString())
                .OrderByDescending(p => p.CreateAt)
                .ProjectTo<PaymentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        //refund lại khi không đủ sớ lượng
        public async Task<bool> RefundPayment(Guid retreatRegId)
        {
            var payment = await _paymentRepository.GetMany(p => p.RetreatRegId == retreatRegId)
                .Include(src => src.RetreatReg)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Payment not found for retreat registration.");

            if (payment.Status != PaymentStatus.Success.ToString())
            {
                throw new ConflictException("Cannot refund a payment that is not completed.");
            }

            var saleId = await PayPalHelper.GetSaleIdAsync(payment.PaypalPaymentId, _appSettings);
            var success = await PayPalHelper.RefundPaymentAsync(saleId, payment.Amount, _appSettings);

            if (success)
            {
                payment.RetreatReg.IsPaid = false;
                payment.Status = PaymentStatus.Refunded.ToString();
                await _unitOfWork.SaveChanges();
            }

            return success;

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

            var flag = _paymentRepository.GetMany(pay => pay.RetreatRegId == retreatRegId);
            if (flag.Any()) 
            {
                _paymentRepository.RemoveRange(flag);
            }

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
            var executeResult = await PayPalHelper.ExecutePaymentAsync(model.PayPalPaymentId, model.PayerId, _appSettings);

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



        public async Task<RefundViewModel> GetRefund(string id)
        {
            return await _refundRepository.GetMany(rf => rf.Id == id)
                .ProjectTo<RefundViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Refund not found");
        }

        public async Task<RefundViewModel> RefundPayment(Guid accountId, CreateRefundModel model)
        {
            decimal returnAmount = 0.8m; // 80% hoàn tiền

            var retreatReg = await _retreatRegistrationRepository.GetMany(reg => reg.Id == model.RetreatRegId)
                                                                .Include(reg => reg.Retreat)
                                                                .Include(reg => reg.Payments)
                                                                .FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat registration not found");

            if (!retreatReg.IsPaid)
            {
                throw new ConflictException("Retreat Registration has not been paid yet");
            }
            if(!retreatReg.Payments.Any(src => src.Status == PaymentStatus.Success.ToString()))
            {
                throw new ConflictException("Refund in progress");
            }

            await CheckAccountIsRegisteredForRetreat(model.RetreatRegId, accountId);

            decimal refundAmount = retreatReg.Retreat.Cost * returnAmount;
            var payout = new PayPalPayoutModel
            {
                EmailPaypal = model.EmailPaypal,
                Amount = (int)refundAmount,
                ParticipantId = accountId,
            };
            var refundResponse = await PayPalHelper.CreatePayoutAsync(payout, _appSettings);

            var refund = new Refund
            {
                Id = refundResponse.BatchHeader.PayoutBatchId,
                RetreatRegId = model.RetreatRegId,
                ParticipantId = accountId,
                TotalAmount = retreatReg.Retreat.Cost,
                RefundAmount = refundAmount,
                RefundReason = model.RefundReason,
                EmailPaypal = model.EmailPaypal,
                Status = refundResponse.BatchHeader.BatchStatus
            };
            _refundRepository.Add(refund);

            foreach (var payment in retreatReg.Payments.Where(pay => pay.Status == PaymentStatus.Success.ToString()))
            {
                payment.Status = PaymentStatus.Refunding.ToString();
            }

            _retreatRegistrationRepository.Update(retreatReg);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetRefund(refundResponse.BatchHeader.PayoutBatchId) : null!;
        }

        public async Task UpdateRefund(string refundId)
        {
            var refund = await _refundRepository.GetMany(r => r.Id == refundId)
                .Include(re => re.RetreatReg)
                    .ThenInclude(reg => reg.Payments)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Refund not found");
            if(refund.Status == "Success")
            {
                return;
            }

            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    await RemoveParticipantFormRetreat(refund.RetreatRegId, refund.ParticipantId);
                    foreach (var payment in refund.RetreatReg.Payments.Where(pay => pay.Status == PaymentStatus.Refunding.ToString()))
                    {
                        payment.Status = PaymentStatus.Refunded.ToString();
                    }
                    refund.Status = "Success";
                    refund.RetreatReg.IsPaid = refund.RetreatReg.TotalParticipants == 0 ? false : true;
                    _refundRepository.Update(refund);
                    await _unitOfWork.SaveChanges();
                    
                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task CheckAccountIsRegisteredForRetreat(Guid retreatRegId, Guid accountId)
        {
            var flag = await _retreatRegistrationParticipantRepository.GetMany(p => p.RetreatRegId == retreatRegId && p.ParticipantId == accountId).FirstOrDefaultAsync();
            if (flag == null)
            {
                throw new ConflictException("This account is not already registered for the retreat.");
            }
        }

        private async Task RemoveParticipantFormRetreat(Guid retreatRegId, Guid participantId)
        {
            var participantToRemove = await _retreatRegistrationParticipantRepository
                .GetMany(par => par.RetreatRegId == retreatRegId && par.ParticipantId == participantId).FirstOrDefaultAsync() ?? throw new NotFoundException("Participant not found in this registration.");

            _retreatRegistrationParticipantRepository.Remove(participantToRemove);

            var retreatReg = await _retreatRegistrationRepository.GetMany(reg => reg.Id == retreatRegId).Include(reg => reg.Retreat).FirstOrDefaultAsync() ?? throw new NotFoundException("Not found");
            retreatReg.TotalParticipants -= 1;
            retreatReg.TotalCost = retreatReg.TotalParticipants * retreatReg.Retreat.Cost;
            retreatReg.Retreat.RemainingSlots += 1;
            _retreatRegistrationRepository.Update(retreatReg);
            await _unitOfWork.SaveChanges();
        }







        //-----------------------------------------------------

        public async Task<ListViewModel<PaymentViewModel>> ViewCustomerPaymentHistory(Guid customerId, PaginationRequestModel pagination)
        {
            var query = _paymentRepository.GetAll().Where(p => p.RetreatReg.CreateBy == customerId && p.Status != "Cancelled");

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(p => p.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var payments = await paginatedQuery.
                ProjectTo<PaymentViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<PaymentViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = payments
            };
        }



    }
}
