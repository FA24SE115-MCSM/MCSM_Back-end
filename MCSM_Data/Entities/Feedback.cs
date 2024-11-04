using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid RetreatId { get; set; }

    public string Content { get; set; } = null!;

    public int Rating { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;
}
