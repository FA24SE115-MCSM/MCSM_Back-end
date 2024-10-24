using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class RetreatRegistrationViewModel
    {
        public Guid Id { get; set; }

        public Guid CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public decimal TotalCost { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPaid { get; set; }

        public virtual Account CreateByNavigation { get; set; }

        //public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual RetreatRegistrationParticipantViewModel RetreatRegistrationParticipant { get; set; } = null!;


    }
}
