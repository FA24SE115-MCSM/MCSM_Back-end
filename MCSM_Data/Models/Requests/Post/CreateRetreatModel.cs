using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatModel
    {
        public string Name { get; set; } = null!;

        public decimal Cost { get; set; }

        public int Capacity { get; set; }

        public int Duration { get; set; }
        public string? Description { get; set; }

        public DateOnly StartDate { get; set; }

        public List<CreateRetreatLearningOutcomeModel> LearningOutcome { get; set; } = new List<CreateRetreatLearningOutcomeModel>();

        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();

    }
}
