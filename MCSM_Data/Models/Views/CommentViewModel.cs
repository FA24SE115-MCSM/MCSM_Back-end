namespace MCSM_Data.Models.Views
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }

        public DateTime? UpdateAt { get; set; }
        public virtual ICollection<ChildCommentViewModel> InverseParentComment { get; set; } = new List<ChildCommentViewModel>();

        public virtual AccountViewModel Account { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }


    public class ChildCommentViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }

        public DateTime? UpdateAt { get; set; }

        public virtual AccountViewModel Account { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
