using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateFeedbackModel
    {
        //public Guid CreatedBy { get; set; }

        public Guid RetreatId { get; set; }

        [Range(1, 5, ErrorMessage = "RetreatRating must be between 1 and 5.")]
        public int RetreatRating { get; set; }

        [Range(1, 5, ErrorMessage = "MonkRating must be between 1 and 5.")]
        public int MonkRating { get; set; }

        [Range(1, 5, ErrorMessage = "RoomRating must be between 1 and 5.")]
        public int RoomRating { get; set; }

        [Range(1, 5, ErrorMessage = "FoodRating must be between 1 and 5.")]
        public int FoodRating { get; set; }

        public string? YourExperience { get; set; }

        public string? Suggestion { get; set; }

        //public DateTime CreateAt { get; set; }
    }
}
