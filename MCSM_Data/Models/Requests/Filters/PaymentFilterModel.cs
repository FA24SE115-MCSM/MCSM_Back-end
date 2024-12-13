using MCSM_Utility.Enums;

namespace MCSM_Data.Models.Requests.Filters
{
    public class PaymentFilterModel
    {
        public Guid? AccountId { get; set; }
        public Guid? RetreatRegistrationId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerName { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}
