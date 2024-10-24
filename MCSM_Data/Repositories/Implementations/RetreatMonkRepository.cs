using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatMonkRepository : Repository<RetreatMonk>, IRetreatMonkRepository
    {
        public RetreatMonkRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
