using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatGroupMemberRepository : Repository<RetreatGroupMember>, IRetreatGroupMemberRepository
    {
        public RetreatGroupMemberRepository(McsmDbContext context) : base(context)
        { 
        }
    }
}
