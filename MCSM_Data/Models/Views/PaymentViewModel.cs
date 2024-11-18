using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class PaymentViewModel
    {
        public string Id { get; set; } = null!;

        public string RetreatName { get; set; } = null!;
        public Guid RetreatRegId { get; set; }

        public string PaypalPaymentId { get; set; } = null!;

        public string PaymentMethod { get; set; } = null!;

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public virtual AccountViewModel Account { get; set; } = null!;
        public virtual RetreatRegistrationViewModel RetreatReg { get; set; } = null!;

    }
}
