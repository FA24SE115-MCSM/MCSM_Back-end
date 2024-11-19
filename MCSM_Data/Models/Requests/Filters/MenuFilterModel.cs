using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Filters
{
    public class MenuFilterModel
    {
        public string? MenuName { get; set; }
        public DateOnly? CookDate { get; set; }
    }
}
