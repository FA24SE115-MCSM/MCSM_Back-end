namespace MCSM_Data.Models.Views
{
    public class RetreatViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

        public int Duration { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public virtual AccountViewModel CreatedBy { get; set; } = null!;

        //public virtual RetreatGroup? RetreatGroup { get; set; }

        //public virtual RetreatLesson? RetreatLesson { get; set; }

        //public virtual RetreatMonk? RetreatMonk { get; set; }

        //public virtual RetreatSchedule? RetreatSchedule { get; set; }

        //public virtual ToolHistory? ToolHistory { get; set; }
    }
}
