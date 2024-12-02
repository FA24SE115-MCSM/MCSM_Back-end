using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Conversation
{
    public Guid Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
