using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatLearningOutcomeRepository : Repository<RetreatLearningOutcome>, IRetreatLearningOutcomeRepository
    {
        public RetreatLearningOutcomeRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
