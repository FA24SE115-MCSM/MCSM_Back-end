namespace MCSM_Data.Models.Views
{
    public class ToolHistoryViewModel
    {
        public Guid Id { get; set; }

        public int NumOfTool { get; set; }

        public DateTime CreateAt { get; set; }

        //public virtual AccountViewModel CreatedBy { get; set; } = null!;

        public virtual RetreatViewModel Retreat { get; set; } = null!;

        public virtual ToolViewModel Tool { get; set; } = null!;
    }
}
