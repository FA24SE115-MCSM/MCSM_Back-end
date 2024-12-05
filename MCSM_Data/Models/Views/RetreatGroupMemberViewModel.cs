namespace MCSM_Data.Models.Views
{
    public class RetreatGroupMemberViewModel
    {
        public Guid Id { get; set; }

        //public Guid GroupId { get; set; }

        //public Guid MemberId { get; set; }

        //public virtual RetreatGroup Group { get; set; } = null!;

        public virtual AccountViewModel Member { get; set; } = null!;
    }
}
