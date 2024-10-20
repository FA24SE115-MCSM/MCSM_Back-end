using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid? ReplyTo { get; set; }

    public string? Content { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Post Post { get; set; } = null!;
}
