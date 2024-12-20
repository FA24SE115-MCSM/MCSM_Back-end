﻿using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public Guid SenderId { get; set; }

    public string Content { get; set; } = null!;
    public bool IsRead { get; set; }

    public DateTime? SendAt { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
