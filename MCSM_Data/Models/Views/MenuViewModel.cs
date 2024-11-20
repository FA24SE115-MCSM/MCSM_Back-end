using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class MenuViewModel
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }

        public DateOnly CookDate { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public ICollection<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }
}
