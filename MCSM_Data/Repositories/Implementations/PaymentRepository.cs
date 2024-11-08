using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
