using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateMenuModel
    {
        public string? MenuName { get; set; }
        public DateOnly? CookDate { get; set; }

        public List<string>? DishName { get; set; }
    }
}
