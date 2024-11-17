﻿using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Dish
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid DishTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Note { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();

    public virtual DishType DishType { get; set; } = null!;

    public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
}
