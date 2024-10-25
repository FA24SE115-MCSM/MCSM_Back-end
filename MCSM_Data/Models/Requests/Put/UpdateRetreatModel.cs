namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateRetreatModel
    {
        public string? Name { get; set; }
        public decimal? Cost { get; set; }


        public int? Capacity { get; set; }

        public int? Duration { get; set; }

        public DateOnly? StartDate { get; set; }

        public string? Status { get; set; }

    }
}
