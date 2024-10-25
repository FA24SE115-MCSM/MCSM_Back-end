using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Content { get; set; } = null!;

    public string? Url { get; set; }

    public string Type { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
