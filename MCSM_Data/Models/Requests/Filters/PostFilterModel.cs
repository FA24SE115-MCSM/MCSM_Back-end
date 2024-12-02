using MCSM_Utility.Enums;

namespace MCSM_Data.Models.Requests.Filters
{
    public class PostFilterModel
    {
        public Guid? AccountId { get; set; }
        public PostStatus? Status { get; set; }

    }
}
