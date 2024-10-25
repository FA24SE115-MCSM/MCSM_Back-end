using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Menu
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public DateOnly CookDate { get; set; }

    public bool IsBreakfast { get; set; }

    public bool IsLunch { get; set; }

    public bool IsDinner { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
}
