using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatGroupRepository : Repository<RetreatGroup>, IRetreatGroupRepository
    {
        public RetreatGroupRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
