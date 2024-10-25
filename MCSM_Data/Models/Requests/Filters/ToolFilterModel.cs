using MCSM_Utility.Enums;

namespace MCSM_Data.Models.Requests.Filters
{
    public class ToolFilterModel
    {
        public string? Name { get; set; }
        public ToolStatus? Status { get; set; }
    }
}
