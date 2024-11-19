using MCSM_Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Filters
{
    public class DishFilterModel
    {
        public string? DishTypeName { get; set; }

        public string? Name { get; set; }
    }

    public class DishTypeFilterModel
    {
        public string? DishTypeName { get; set; }
    }
}
