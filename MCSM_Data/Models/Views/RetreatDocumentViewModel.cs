namespace MCSM_Data.Models.Views
{
    public class RetreatDocumentViewModel
    {
        public Guid Id { get; set; }
        public string? FileName { get; set; }

        public string Url { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
