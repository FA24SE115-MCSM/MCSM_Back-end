namespace MCSM_Data.Models.Views
{
    public class RetreatToolViewModel
    {
        public int Quantity { get; set; }
        public virtual ToolViewModel Tool { get; set; } = null!;
    }
}
