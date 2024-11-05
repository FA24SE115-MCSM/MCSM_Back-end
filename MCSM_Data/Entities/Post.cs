using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Post
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public string? Content { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? UpdateAt { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}
