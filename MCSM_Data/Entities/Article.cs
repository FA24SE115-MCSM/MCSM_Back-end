using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Article
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string Banner { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;
}
