using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateDishModel
    {
        public string? Status { get; set; }
        public string? Note { get; set; }
        //public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string>? IngredientNames { get; set; }
    }
}
