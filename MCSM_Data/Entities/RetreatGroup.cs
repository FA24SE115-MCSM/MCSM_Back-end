﻿using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatGroup
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public Guid? MonkId { get; set; }

    public Guid RoomId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupSchedule> GroupSchedules { get; set; } = new List<GroupSchedule>();

    public virtual Account? Monk { get; set; }

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual ICollection<RetreatGroupMember> RetreatGroupMembers { get; set; } = new List<RetreatGroupMember>();

    public virtual Room Room { get; set; } = null!;
}
