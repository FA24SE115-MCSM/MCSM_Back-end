namespace MCSM_Data.Models.Views
{
    public class RetreatLearningOutcomeViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }

        public string? Description { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
