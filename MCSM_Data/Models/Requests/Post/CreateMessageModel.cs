﻿namespace MCSM_Data.Models.Requests.Post
{
    public class CreateMessageModel
    {
        public Guid ConversationId { get; set; }

        public Guid SenderId { get; set; }

        public string Content { get; set; } = null!;
    }
}