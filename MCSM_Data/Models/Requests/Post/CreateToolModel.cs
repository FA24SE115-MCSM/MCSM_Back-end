using MCSM_Utility.Enums;
using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateToolModel
    {

        public string Name { get; set; } = null!;

        public int TotalTool { get; set; }

        public ToolStatus Status { get; set; }

        public IFormFile Image { get; set; } = null!;
    }
}
