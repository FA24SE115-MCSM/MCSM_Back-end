using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Like
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
