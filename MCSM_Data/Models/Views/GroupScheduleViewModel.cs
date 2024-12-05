using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class GroupScheduleViewModel
    {
        public Guid Id { get; set; }
        public Guid RetreatScheduleId { get; set; }

        public Guid? GroupId { get; set; }
        public string GroupName { get; set; }

        //public Guid? UsedRoomId { get; set; }
        public string RoomName { get; set; }
        public DateOnly LessionDate { get; set; }
        public TimeOnly LessonStart { get; set; }
        public TimeOnly LessonEnd { get; set; }

        public DateTime CreateAt { get; set; }

        //public virtual RetreatGroup? Group { get; set; }

        //public virtual RetreatSchedule RetreatSchedule { get; set; } = null!;

        //public virtual Room? UsedRoom { get; set; }
    }
}
