using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatRegistrationParticipantViewModel
    {
        public Guid Id { get; set; }

        public Guid RetreatId { get; set; }

        public string PractitionerMail { get; set; }

        public virtual RetreatViewModel Retreat { get; set; } = null!;

        public virtual AccountViewModel Practitioner { get; set; } = null!;


    }
}
