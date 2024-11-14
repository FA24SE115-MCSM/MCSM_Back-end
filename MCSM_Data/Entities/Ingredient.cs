using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Ingredient
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
