using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class RetreatGroupViewModel
    {
        public Guid Id { get; set; }

        public Guid RetreatId { get; set; }

        public Guid MonkId { get; set; }

        public string Name { get; set; } = null!;

        public virtual AccountViewModel Monk { get; set; } = null!;

        public virtual RetreatViewModel Retreat { get; set; } = null!;

        //public virtual ICollection<RetreatGroupMember> RetreatGroupMembers { get; set; } = new List<RetreatGroupMember>();

        //public virtual ICollection<RetreatGroupMessage> RetreatGroupMessages { get; set; } = new List<RetreatGroupMessage>();

        public virtual ICollection<RetreatScheduleViewModel> RetreatSchedules { get; set; } = new List<RetreatScheduleViewModel>();
    }
}
