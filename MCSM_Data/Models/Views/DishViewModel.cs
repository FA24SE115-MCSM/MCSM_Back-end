using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class DishViewModel
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }

        public Guid DishTypeId { get; set; }
        public string DishTypeName { get; set; }

        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        public string? Note { get; set; }

        //public Guid? MenuId { get; set; }


        //public virtual Account CreatedByNavigation { get; set; } = null!;

        //public virtual DishType DishType { get; set; } = null!;


        //public virtual Menu? Menu { get; set; }
    }

    public class DishTypeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
