using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Ingredient
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual Allergy? Allergy { get; set; }

    public virtual DishIngredient? DishIngredient { get; set; }
}
