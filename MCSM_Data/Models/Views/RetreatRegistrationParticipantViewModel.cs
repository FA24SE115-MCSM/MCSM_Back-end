using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatRegistrationParticipantViewModel
    {
        public Guid Id { get; set; }

        public Guid RetreatRegId { get; set; }

        public string? PractitionerMail { get; set; }

        //public virtual AccountViewModel Participant { get; set; } = null!;
        //public virtual RetreatRegistrationViewModel RetreatReg { get; set; } = null!;


    }
}
