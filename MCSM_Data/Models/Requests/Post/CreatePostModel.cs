using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreatePostModel
    {
        public string Content { get; set; } = null!;
        public List<IFormFile>? Images { get; set; }
    }
}
