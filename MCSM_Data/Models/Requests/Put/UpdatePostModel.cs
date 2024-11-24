using MCSM_Utility.Enums;
using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdatePostModel
    {
        public string? Content { get; set; }
        public List<IFormFile>? Images { get; set; }
        public PostStatus? Status { get; set; }
    }
}
