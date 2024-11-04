using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Lesson
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<RetreatLesson> RetreatLessons { get; set; } = new List<RetreatLesson>();
}
