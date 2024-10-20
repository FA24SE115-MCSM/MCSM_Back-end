using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatGroup
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public Guid MonkId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Account Monk { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual RetreatGroupMember? RetreatGroupMember { get; set; }

    public virtual RetreatGroupMessage? RetreatGroupMessage { get; set; }

    public virtual RetreatSchedule? RetreatSchedule { get; set; }
}
