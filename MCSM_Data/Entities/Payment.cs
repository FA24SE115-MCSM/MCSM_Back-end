using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid RetreatRegId { get; set; }

    public DateTime CreateAt { get; set; }

    public Guid PaypalOrderId { get; set; }

    public string Status { get; set; } = null!;

    public virtual RetreatRegistration RetreatReg { get; set; } = null!;
}
