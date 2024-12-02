namespace MCSM_Data.Entities;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid AccountId { get; set; }

    public string? Content { get; set; }

    public Guid? ParentCommentId { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual Post Post { get; set; } = null!;
}
