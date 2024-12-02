using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
