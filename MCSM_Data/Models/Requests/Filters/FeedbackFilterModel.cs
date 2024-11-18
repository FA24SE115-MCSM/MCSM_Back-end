using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Filters
{
    public class FeedbackFilterModel
    {
        public string? AccountEmail { get; set; }
        public string? RetreatName { get; set; }
        public int? RetreatRating { get; set; }
        public bool IsDeleted { get; set; }
    }
}
