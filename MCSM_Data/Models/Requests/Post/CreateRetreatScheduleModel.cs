using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatScheduleModel
    {
        public Guid RetreatId { get; set; }
        public Guid GroupId { get; set; }
        public Guid RetreatLessionId { get; set; }
        public string? RoomName { get; set; }
        public DateOnly LessionDate { get; set; }
        public TimeOnly LessionStart { get; set; }
        public TimeOnly LessonEnd { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
