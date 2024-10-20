using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatGroupMessage
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid GroupId { get; set; }

    public Guid? ReplyTo { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual RetreatGroup Group { get; set; } = null!;
}
