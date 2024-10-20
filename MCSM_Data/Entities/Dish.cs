using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Dish
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid DishTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsHalal { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual DishIngredient? DishIngredient { get; set; }

    public virtual DishType DishType { get; set; } = null!;

    public virtual MenuDish? MenuDish { get; set; }
}
