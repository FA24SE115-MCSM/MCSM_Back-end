using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatGroupViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual AccountViewModel? Monk { get; set; }

        //public virtual Retreat Retreat { get; set; } = null!;

        public virtual ICollection<RetreatGroupMemberViewModel> RetreatGroupMembers { get; set; } = new List<RetreatGroupMemberViewModel>();

        //public virtual ICollection<RetreatGroupMessage> RetreatGroupMessages { get; set; } = new List<RetreatGroupMessage>();

        public virtual RoomViewModel Room { get; set; } = null!;
    }
}
