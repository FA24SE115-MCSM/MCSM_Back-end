using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Level
{
    public Guid AccountId { get; set; }

    public string RoleType { get; set; } = null!;

    public int RankLevel { get; set; }

    public string? RankName { get; set; }

    public virtual Account Account { get; set; } = null!;
}
