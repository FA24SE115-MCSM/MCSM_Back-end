using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatRegistrationParticipantModel
    {
        public Guid RetreatRegId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
