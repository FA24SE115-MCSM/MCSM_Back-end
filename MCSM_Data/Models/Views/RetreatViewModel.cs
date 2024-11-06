using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }

        public int Capacity { get; set; }

        public int Duration { get; set; }

        
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string? Description { get; set; }

        public virtual AccountViewModel CreatedBy { get; set; } = null!;
        public virtual ICollection<RetreatImageViewModel> RetreatImages { get; set; } = new List<RetreatImageViewModel>();
        public virtual ICollection<RetreatDocumentViewModel> RetreatDocuments { get; set; } = new List<RetreatDocumentViewModel>();
        public virtual ICollection<RetreatLearningOutcomeViewModel> RetreatLearningOutcomes { get; set; } = new List<RetreatLearningOutcomeViewModel>();
        //public virtual RetreatGroup? RetreatGroup { get; set; }

        //public virtual RetreatLesson? RetreatLesson { get; set; }

        public virtual ICollection<RetreatMonkViewModel> RetreatMonks { get; set; } = new List<RetreatMonkViewModel>();

        //public virtual RetreatSchedule? RetreatSchedule { get; set; }

        //public virtual ToolHistory? ToolHistory { get; set; }
    }
}
