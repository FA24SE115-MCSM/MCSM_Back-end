using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class PostImage
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public string Url { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Post Post { get; set; } = null!;
}
