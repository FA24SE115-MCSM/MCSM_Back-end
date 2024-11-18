using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class IngredientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateIngredientModel
    {
        public string Name { get; set; } = null!;
    }
}
