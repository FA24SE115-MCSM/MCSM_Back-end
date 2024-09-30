using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public ProfileRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
