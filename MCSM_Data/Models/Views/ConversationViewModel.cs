namespace MCSM_Data.Models.Views
{
    public class ConversationViewModel
    {
        public Guid Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ConversationParticipantViewModel> ConversationParticipants { get; set; } = new List<ConversationParticipantViewModel>();

        public virtual ICollection<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();

    }
}
