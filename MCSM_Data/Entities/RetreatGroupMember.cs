using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatGroupMember
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public Guid MemberId { get; set; }

    public virtual RetreatGroup Group { get; set; } = null!;

    public virtual Account Member { get; set; } = null!;
}
