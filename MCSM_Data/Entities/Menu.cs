using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Menu
{
    public Guid Id { get; set; }

    public string? MenuName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateOnly CookDate { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
}
