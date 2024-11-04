using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatRegistrationParticipant
{
    public Guid Id { get; set; }

    public Guid ParticipantId { get; set; }

    public Guid RetreatRegId { get; set; }

    public virtual Account Participant { get; set; } = null!;

    public virtual RetreatRegistration RetreatReg { get; set; } = null!;
}
