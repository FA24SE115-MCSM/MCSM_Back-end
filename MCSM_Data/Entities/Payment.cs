using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Payment
{
    public string Id { get; set; } = null!;

    public Guid RetreatRegId { get; set; }

    public string PaypalOrderId { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual RetreatRegistration RetreatReg { get; set; } = null!;
}
