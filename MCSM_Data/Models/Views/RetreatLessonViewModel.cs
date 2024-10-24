using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatLessonViewModel
    {
        public Guid Id { get; set; }

        public Guid RetreatId { get; set; }

        public Guid LessonId { get; set; }

        public string LessonTitle { get; set; } = null!;

        public Guid AuthorId { get; set; }

        public string AuthorFirstName { get; set; } = null!;

        public string AuthorLastName { get; set; } = null!;
    }
}