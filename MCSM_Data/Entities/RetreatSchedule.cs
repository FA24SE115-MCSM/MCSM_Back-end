using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatSchedule
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    //public Guid GroupId { get; set; }

    public Guid? RetreatLessonId { get; set; }

    public Guid? UsedRoomId { get; set; }

    public DateOnly LessonDate { get; set; }

    public TimeOnly LessonStart { get; set; }

    public TimeOnly LessonEnd { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual RetreatGroup Group { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual RetreatLesson? RetreatLesson { get; set; }

    public virtual Room? UsedRoom { get; set; }
}
