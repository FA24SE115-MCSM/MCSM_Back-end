using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Repositories.Implementations
{
    public class RetreatRegistrationParticipantRepository : Repository<RetreatRegistrationParticipant>, IRetreatRegistrationParticipantRepository
    {
        public RetreatRegistrationParticipantRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
