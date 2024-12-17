using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatTool
{
    public Guid RetreatId { get; set; }

    public Guid ToolId { get; set; }

    public int Quantity { get; set; }

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual Tool Tool { get; set; } = null!;
}
