using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateGroupScheduleModel
    {
        public Guid? RetreatScheduleId { get; set; }
        public string? RoomName { get; set; }
    }
}
