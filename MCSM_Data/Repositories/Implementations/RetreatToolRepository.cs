using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatToolRepository : Repository<RetreatTool>, IRetreatToolRepository
    {
        public RetreatToolRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
