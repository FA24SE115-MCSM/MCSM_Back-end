using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatLearningOutcome
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public string Title { get; set; } = null!;

    public string? SubTitle { get; set; }

    public string? Description { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Retreat Retreat { get; set; } = null!;
}
