using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Filters
{
    public class GroupScheduleFilterModel
    {
        public Guid? GroupId { get; set; }
        public string? RoomName { get; set; }
        public DateOnly? LessionDate { get; set; }
    }
}
