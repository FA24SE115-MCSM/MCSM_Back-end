using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class GroupSchedule
{
    public Guid Id { get; set; }

    public Guid RetreatScheduleId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? UsedRoomId { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual RetreatGroup? Group { get; set; }

    public virtual RetreatSchedule RetreatSchedule { get; set; } = null!;

    public virtual Room? UsedRoom { get; set; }
}
