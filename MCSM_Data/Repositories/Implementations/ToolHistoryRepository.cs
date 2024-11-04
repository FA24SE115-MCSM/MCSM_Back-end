using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class ToolHistoryRepository : Repository<ToolHistory>, IToolHistoryRepository
    {
        public ToolHistoryRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
