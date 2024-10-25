using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatMonk
{
    public Guid Id { get; set; }

    public Guid MonkId { get; set; }

    public Guid RetreatId { get; set; }

    public virtual Account Monk { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;
}
