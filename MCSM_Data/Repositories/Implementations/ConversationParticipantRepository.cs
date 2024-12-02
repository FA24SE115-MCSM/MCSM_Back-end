using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class ConversationParticipantRepository : Repository<ConversationParticipant>, IConversationParticipantRepository
    {
        public ConversationParticipantRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
