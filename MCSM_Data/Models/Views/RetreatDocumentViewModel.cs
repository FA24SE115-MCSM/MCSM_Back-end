namespace MCSM_Data.Models.Views
{
    public class RetreatDocumentViewModel
    {
        public class RetreatImageViewModel
        {
            public Guid Id { get; set; }

            public string Url { get; set; } = null!;

            public DateTime CreateAt { get; set; }
        }
    }
}
