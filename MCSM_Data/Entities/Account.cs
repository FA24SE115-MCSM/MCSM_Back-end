using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string VerifyToken { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<DeviceToken> DeviceTokens { get; set; } = new List<DeviceToken>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Level? Level { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual ICollection<RetreatGroupMember> RetreatGroupMembers { get; set; } = new List<RetreatGroupMember>();

    public virtual ICollection<RetreatGroupMessage> RetreatGroupMessages { get; set; } = new List<RetreatGroupMessage>();

    public virtual ICollection<RetreatGroup> RetreatGroups { get; set; } = new List<RetreatGroup>();

    public virtual ICollection<RetreatMonk> RetreatMonks { get; set; } = new List<RetreatMonk>();

    public virtual ICollection<RetreatRegistrationParticipant> RetreatRegistrationParticipants { get; set; } = new List<RetreatRegistrationParticipant>();

    public virtual ICollection<RetreatRegistration> RetreatRegistrations { get; set; } = new List<RetreatRegistration>();

    public virtual ICollection<Retreat> Retreats { get; set; } = new List<Retreat>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();
}
