using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Tool
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int TotalTool { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual ICollection<RetreatTool> RetreatTools { get; set; } = new List<RetreatTool>();

    public virtual ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();
}
