using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatRegistrationRepository : Repository<RetreatRegistration>, IRetreatRegistrationRepository
    {
        public RetreatRegistrationRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
