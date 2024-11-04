using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Rank
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public int RankLevel { get; set; }

    public string? RankName { get; set; }

    public virtual Account Account { get; set; } = null!;
}
