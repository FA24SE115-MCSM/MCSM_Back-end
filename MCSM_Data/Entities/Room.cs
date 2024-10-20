using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Room
{
    public Guid Id { get; set; }

    public Guid RoomTypeId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public bool IsActive { get; set; }

    public virtual RetreatSchedule? RetreatSchedule { get; set; }

    public virtual RoomType RoomType { get; set; } = null!;
}
