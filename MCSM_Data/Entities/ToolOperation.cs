using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class ToolOperation
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsIncrement { get; set; }

    public virtual ToolHistory? ToolHistory { get; set; }
}
