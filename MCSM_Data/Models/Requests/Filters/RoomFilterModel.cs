using MCSM_Utility.Enums;

namespace MCSM_Data.Models.Requests.Filters
{
    public class RoomFilterModel
    {
        public string? Name { get; set; }

        public RoomStatus? Status { get; set; }
    }
}
