using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class RetreatScheduleViewModel
    {
        public Guid Id { get; set; }
        public Guid RetreatId { get; set; }
        //public string GroupName { get; set; }
        public string LessonTitle { get; set; }
        public string RoomName { get; set; }
        public DateOnly LessonDate { get; set; }
        public TimeOnly LessonStart { get; set; }
        public TimeOnly LessonEnd { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual RoomViewModel UsedRoom { get; set; }
    }
}
