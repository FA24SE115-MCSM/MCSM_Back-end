using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Lesson
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual RetreatLesson? RetreatLesson { get; set; }
}
