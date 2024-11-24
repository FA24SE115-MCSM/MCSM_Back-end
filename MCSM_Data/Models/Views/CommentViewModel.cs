namespace MCSM_Data.Models.Views
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }

        public DateTime? UpdateAt { get; set; }

        public bool IsDeleted { get; set; }
        public virtual AccountViewModel Account { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
