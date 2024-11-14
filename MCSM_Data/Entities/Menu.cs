using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Menu
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid DishId { get; set; }

    public DateOnly CookDate { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Dish Dish { get; set; } = null!;

    public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
}
