using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatLessonRepository : Repository<RetreatLesson>, IRetreatLessonRepository
    {
        public RetreatLessonRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
