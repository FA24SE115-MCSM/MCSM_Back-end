using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateDishModel
    {

        public string DishTypeName { get; set; }

        public string Name { get; set; } = null!;

        public string? Note { get; set; }
    }

    public class CreateDishTypeModel
    {
        public string Name { get; set; } = null!;

    }
}
