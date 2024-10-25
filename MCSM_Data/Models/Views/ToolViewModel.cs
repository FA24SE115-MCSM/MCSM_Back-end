namespace MCSM_Data.Models.Views
{
    public class ToolViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public int TotalTool { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
