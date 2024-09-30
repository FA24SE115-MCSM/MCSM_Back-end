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

    public virtual Profile? Profile { get; set; }

    public virtual Role Role { get; set; } = null!;
}
