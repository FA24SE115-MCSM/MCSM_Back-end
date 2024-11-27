using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RoomViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

        public string Status { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual ICollection<RetreatGroupMemberViewModel> RetreatGroupMembers { get; set; } = new List<RetreatGroupMemberViewModel>();


        public virtual RoomTypeViewModel RoomType { get; set; } = null!;
    }
}
