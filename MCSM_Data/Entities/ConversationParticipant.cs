using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class ConversationParticipant
{
    public Guid ConversationId { get; set; }

    public Guid AccountId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Conversation Conversation { get; set; } = null!;
}
