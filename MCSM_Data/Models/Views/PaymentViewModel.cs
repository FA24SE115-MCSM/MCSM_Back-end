﻿namespace MCSM_Data.Models.Views
{
    public class PaymentViewModel
    {
        public string Id { get; set; } = null!;

        public string RetreatName { get; set; } = null!;
        public Guid RetreatRegId { get; set; }
        public List<string> RegisteredEmails { get; set; }
        public List<string> RegisteredPhoneNumber { get; set; }
        public string PaypalPaymentId { get; set; } = null!;

        public string PaymentMethod { get; set; } = null!;

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }

        //public virtual AccountViewModel Account { get; set; } = null!;

    }
}
