namespace MCSM_Data.Models.Views
{
    public class PostImageViewModel
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;

        public DateTime CreateAt { get; set; }

    }
}
