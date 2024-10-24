using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateLessonModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
