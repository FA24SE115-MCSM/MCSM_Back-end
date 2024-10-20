using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class ToolHistory
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid RetreatId { get; set; }

    public Guid ToolId { get; set; }

    public Guid ToolOpId { get; set; }

    public int NumOfTool { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual Tool Tool { get; set; } = null!;

    public virtual ToolOperation ToolOp { get; set; } = null!;
}
