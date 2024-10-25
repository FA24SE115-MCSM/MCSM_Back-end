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

    public virtual ICollection<RetreatGroupMember> RetreatGroupMembers { get; set; } = new List<RetreatGroupMember>();

    public virtual ICollection<RetreatGroupMessage> RetreatGroupMessages { get; set; } = new List<RetreatGroupMessage>();

    public virtual ICollection<RetreatSchedule> RetreatSchedules { get; set; } = new List<RetreatSchedule>();
}
