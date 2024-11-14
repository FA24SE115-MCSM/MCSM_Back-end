using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Payment
{
    public string Id { get; set; } = null!;

    public Guid AccountId { get; set; }

    public Guid RetreatRegId { get; set; }

    public string PaypalPaymentId { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual RetreatRegistration RetreatReg { get; set; } = null!;
}
