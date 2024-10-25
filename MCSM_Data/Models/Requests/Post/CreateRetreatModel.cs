namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatModel
    {
        public string Name { get; set; } = null!;

        public decimal Cost { get; set; }

        public int Capacity { get; set; }

        public int Duration { get; set; }

        public DateOnly StartDate { get; set; }
        public string Status { get; set; } = null!;

    }
}
