using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class DishType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
