using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public int? Level { get; set; }

    public string VerifyToken { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Allergy? Allergy { get; set; }

    public virtual Article? Article { get; set; }

    public virtual DeviceToken? DeviceToken { get; set; }

    public virtual Dish? Dish { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual Like? Like { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Notification? Notification { get; set; }

    public virtual Post? Post { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual Retreat? Retreat { get; set; }

    public virtual RetreatGroup? RetreatGroup { get; set; }

    public virtual RetreatGroupMember? RetreatGroupMember { get; set; }

    public virtual RetreatGroupMessage? RetreatGroupMessage { get; set; }

    public virtual RetreatMonk? RetreatMonk { get; set; }

    public virtual RetreatRegistration? RetreatRegistrationMonk { get; set; }

    public virtual RetreatRegistration? RetreatRegistrationPractitioner { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ToolHistory? ToolHistory { get; set; }
}
