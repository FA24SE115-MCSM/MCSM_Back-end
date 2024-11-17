using MCSM_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Views
{
    public class FeedbackViewModel
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid RetreatId { get; set; }

        public int RetreatRating { get; set; }

        public int MonkRating { get; set; }

        public int RoomRating { get; set; }

        public int FoodRating { get; set; }

        public string? YourExperience { get; set; }

        public string? Suggestion { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual AccountViewModel CreatedByNavigation { get; set; } = null!;

        public virtual RetreatViewModel Retreat { get; set; } = null!;
    }
}
