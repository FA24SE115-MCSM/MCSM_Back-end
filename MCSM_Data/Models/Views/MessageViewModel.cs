namespace MCSM_Data.Models.Views
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime? SendAt { get; set; }
        public virtual AccountViewModel Sender { get; set; } = null!;
    }
}
