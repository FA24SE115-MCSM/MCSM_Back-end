using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatRegistrationModel
    {
        public Guid CreateBy {  get; set; }

        public Guid RetreatId { get; set; }

        public decimal TotalCost { get; set; }

        public int? TotalParticipants { get; set; }

        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }

        [DefaultValue(false)]
        public bool? IsPaid { get; set; }
    }
}
