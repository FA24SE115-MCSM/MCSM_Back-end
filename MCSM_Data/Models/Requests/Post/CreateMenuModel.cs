using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateMenuModel
    {
        public DateOnly CookDate { get; set; }

        //public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
        public virtual List<string> DishName { get; set; } = new List<string>();
    }
}
