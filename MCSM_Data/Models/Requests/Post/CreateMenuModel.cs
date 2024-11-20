using MCSM_Data.Entities;
using MCSM_Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateMenuModel
    {
        public string? MenuName { get; set; }
        public DateOnly CookDate { get; set; }

        public virtual List<string> DishName { get; set; } = new List<string>();
    }
}
