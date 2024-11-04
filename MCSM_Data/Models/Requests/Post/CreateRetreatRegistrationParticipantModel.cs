using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatRegistrationParticipantModel
    {
        public Guid retreatRegId { get; set; }
        public List<string> participantEmail { get; set; }
    }
}
