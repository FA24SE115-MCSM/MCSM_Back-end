using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatFile
{
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public string Url { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public string? FileName { get; set; }

    public virtual Retreat Retreat { get; set; } = null!;
}
