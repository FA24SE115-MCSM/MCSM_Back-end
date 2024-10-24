using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatRegistrationParticipantModel
    {
        public required List<Guid> participantId {  get; set; }
        public Guid retreatRegId { get; set; }
    }
}
