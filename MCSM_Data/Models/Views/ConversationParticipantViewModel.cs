namespace MCSM_Data.Models.Views
{
    public class ConversationParticipantViewModel
    {
        public DateTime? JoinedAt { get; set; }
        public virtual AccountViewModel Account { get; set; } = null!;

    }
}
