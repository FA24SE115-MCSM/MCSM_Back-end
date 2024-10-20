using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatLesson
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public Guid LessonId { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual RetreatSchedule? RetreatSchedule { get; set; }
}
