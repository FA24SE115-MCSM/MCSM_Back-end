using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class MenuDish
{
    public Guid Id { get; set; }

    public Guid? MenuId { get; set; }

    public Guid? DishId { get; set; }

    public virtual Dish? Dish { get; set; }

    public virtual Menu? Menu { get; set; }
}
