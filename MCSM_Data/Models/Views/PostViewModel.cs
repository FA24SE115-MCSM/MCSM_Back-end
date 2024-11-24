namespace MCSM_Data.Models.Views
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string? Content { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? UpdateAt { get; set; }

        public DateTime CreateAt { get; set; }

        public virtual ICollection<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public virtual AccountViewModel CreatedByNavigation { get; set; } = null!;

        public virtual ICollection<PostImageViewModel> PostImages { get; set; } = new List<PostImageViewModel>();

        public virtual ICollection<ReactionViewModel> Reactions { get; set; } = new List<ReactionViewModel>();
    }
}
