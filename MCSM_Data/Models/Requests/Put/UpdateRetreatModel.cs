using MCSM_Data.Models.Requests.Post;
using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateRetreatModel
    {
        public string? Name { get; set; }
        public decimal? Cost { get; set; }

        public int? Capacity { get; set; }

        public int? Duration { get; set; }
        public string? Description { get; set; }

        public DateOnly? StartDate { get; set; }

        //public string? Status { get; set; }
        public List<CreateRetreatLearningOutcomeModel>? LearningOutcome { get; set; } 
        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Documents { get; set; }

    }
}
