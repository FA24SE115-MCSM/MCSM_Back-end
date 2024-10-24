using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateLessonModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;        
    }
}
