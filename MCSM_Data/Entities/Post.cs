using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Post
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string? Content { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Like? Like { get; set; }
}
