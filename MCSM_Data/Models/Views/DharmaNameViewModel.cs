namespace MCSM_Data.Models.Views
{
    public class DharmaNameViewModel
    {
        public string Name { get; set; } = null!;
        public string DharmaName { get; set; } = null!;
        public string RetreatName { get; set; } = null!;
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
