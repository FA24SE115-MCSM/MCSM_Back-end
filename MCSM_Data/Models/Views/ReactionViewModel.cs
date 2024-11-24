namespace MCSM_Data.Models.Views
{
    public class ReactionViewModel
    {
        public Guid Id { get; set; }

        public string ReactionType { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public virtual AccountViewModel Account { get; set; } = null!;

    }
}
