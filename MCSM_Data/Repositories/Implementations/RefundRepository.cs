using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class RefundRepository : Repository<Refund>, IRefundRepository
    {
        public RefundRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
