namespace MCSM_Data.Models.Views
{
    public class LessonViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CreatorId { get; set; } 

        public string CreatorFirstName { get; set; } = null!;

        public string CreatorLastName { get; set; } = null!;
    }
}
