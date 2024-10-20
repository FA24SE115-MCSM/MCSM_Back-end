using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatRepository : Repository<Retreat>, IRetreatRepository
    {
        public RetreatRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
