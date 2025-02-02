﻿using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Room
{
    public Guid Id { get; set; }

    public Guid RoomTypeId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual ICollection<GroupSchedule> GroupSchedules { get; set; } = new List<GroupSchedule>();

    public virtual RetreatGroup? RetreatGroup { get; set; }

    public virtual RoomType RoomType { get; set; } = null!;
}
