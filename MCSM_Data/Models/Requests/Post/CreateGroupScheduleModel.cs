using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateGroupScheduleModel
    {
        //public Guid Id { get; set; }

        public Guid RetreatScheduleId { get; set; }

        public Guid GroupId { get; set; }

        public string RoomName { get; set; }

        //public DateTime CreateAt { get; set; }
    }
}
