using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Allergy
{
    public Guid Id { get; set; }

    public Guid IngredientId { get; set; }

    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
