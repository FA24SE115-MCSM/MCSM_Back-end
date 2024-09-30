using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Profile
{
    public Guid AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Avatar { get; set; }

    public virtual Account Account { get; set; } = null!;
}
