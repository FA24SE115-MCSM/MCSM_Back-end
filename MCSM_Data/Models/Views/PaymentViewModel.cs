using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class PaymentViewModel
    {
        public string Id { get; set; } = null!;

        public Guid RetreatRegId { get; set; }

        public string PaypalOrderId { get; set; } = null!;

        public string PaymentMethod { get; set; } = null!;

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
