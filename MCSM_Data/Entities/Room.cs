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

    public DateTime CreateAt { get; set; }

    public virtual ICollection<RetreatSchedule> RetreatSchedules { get; set; } = new List<RetreatSchedule>();

    public virtual RoomType RoomType { get; set; } = null!;
}
