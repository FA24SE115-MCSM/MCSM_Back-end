using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRetreatMonkModel
    {
        public Guid MonkId { get; set; }

        public Guid RetreatId { get; set; }
    }
}
