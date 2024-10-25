using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RoomType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
