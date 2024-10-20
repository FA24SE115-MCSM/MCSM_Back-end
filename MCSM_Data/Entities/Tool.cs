using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Tool
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int TotalTool { get; set; }

    public int AvailableTool { get; set; }

    public bool IsActive { get; set; }

    public virtual ToolHistory? ToolHistory { get; set; }
}
