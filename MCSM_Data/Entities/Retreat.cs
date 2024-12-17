using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Retreat
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int Capacity { get; set; }

    public int RemainingSlots { get; set; }

    public string? DharmaNamePrefix { get; set; }

    public int Duration { get; set; }

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RetreatFile> RetreatFiles { get; set; } = new List<RetreatFile>();

    public virtual ICollection<RetreatGroup> RetreatGroups { get; set; } = new List<RetreatGroup>();

    public virtual ICollection<RetreatLearningOutcome> RetreatLearningOutcomes { get; set; } = new List<RetreatLearningOutcome>();

    public virtual ICollection<RetreatLesson> RetreatLessons { get; set; } = new List<RetreatLesson>();

    public virtual ICollection<RetreatMonk> RetreatMonks { get; set; } = new List<RetreatMonk>();

    public virtual ICollection<RetreatRegistration> RetreatRegistrations { get; set; } = new List<RetreatRegistration>();

    public virtual ICollection<RetreatSchedule> RetreatSchedules { get; set; } = new List<RetreatSchedule>();

    public virtual ICollection<RetreatTool> RetreatTools { get; set; } = new List<RetreatTool>();

    public virtual ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();
}
