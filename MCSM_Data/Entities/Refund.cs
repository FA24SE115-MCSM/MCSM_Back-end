using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Refund
{
    public string Id { get; set; } = null!;

    public Guid RetreatRegId { get; set; }

    public Guid ParticipantId { get; set; }

    public decimal RefundAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string RefundReason { get; set; } = null!;

    public string EmailPaypal { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Account Participant { get; set; } = null!;

    public virtual RetreatRegistration RetreatReg { get; set; } = null!;
}
