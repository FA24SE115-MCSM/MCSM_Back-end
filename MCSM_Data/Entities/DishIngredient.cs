using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class DishIngredient
{
    public Guid Id { get; set; }

    public Guid DishId { get; set; }

    public Guid IngredientId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
