using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatRegistration
{
    public Guid Id { get; set; }

    public Guid MonkId { get; set; }

    public Guid PractitionerId { get; set; }

    public virtual Account Monk { get; set; } = null!;

    public virtual Account Practitioner { get; set; } = null!;
}
