using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class RefundViewModel
    {
        public string Id { get; set; } = null!;

        public Guid RetreatRegId { get; set; }

        public decimal RefundAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string RefundReason { get; set; } = null!;

        public string EmailPaypal { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public virtual AccountViewModel Participant { get; set; } = null!;
    }
}
