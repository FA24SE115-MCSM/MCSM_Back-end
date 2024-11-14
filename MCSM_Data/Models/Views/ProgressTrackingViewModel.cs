using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class ProgressTrackingViewModel
    {
        public Guid RetreatId { get; set; }
        public string RetreatName { get; set; }
        //public decimal CurrentProgress { get; set; }
        public string CurrentProgress { get; set; }
        public int Duration { get; set; }
        public int CurrentDay { get; set; }
    }
}
