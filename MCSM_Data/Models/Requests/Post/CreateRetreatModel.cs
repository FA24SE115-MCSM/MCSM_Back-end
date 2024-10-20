namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatModel
    {
        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

        public int Duration { get; set; }

        public DateOnly StartDate { get; set; }

    }
}
