using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatFileRepository : Repository<RetreatFile>, IRetreatFileRepository
    {
        public RetreatFileRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
