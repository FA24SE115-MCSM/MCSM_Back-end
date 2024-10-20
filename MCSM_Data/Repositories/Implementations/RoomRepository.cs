using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
