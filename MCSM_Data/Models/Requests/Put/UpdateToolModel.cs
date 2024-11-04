using MCSM_Utility.Enums;
using Microsoft.AspNetCore.Http;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateToolModel
    {

        public string? Name { get; set; }
        public int? TotalTool { get; set; }
        public ToolStatus? Status { get; set; }
        public IFormFile? Image { get; set; }
    }
}
