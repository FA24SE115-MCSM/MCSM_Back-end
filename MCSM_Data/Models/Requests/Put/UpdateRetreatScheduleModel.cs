using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateRetreatScheduleModel
    {
        public Guid RetreatId { get; set; }
        public Guid GroupId { get; set; }
        public Guid? RetreatLessonId { get; set; }
        public Guid? UsedRoomId { get; set; }

        public DateOnly LessonDate { get; set; }

        public TimeOnly LessonStart { get; set; }

        public TimeOnly LessonEnd { get; set; }
    }
}
