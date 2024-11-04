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
        public Guid RetreatId { get; set; }
        public string? RetreatName { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalParticipants { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPaid { get; set; }
    }

    public class ActiveRetreatRegistrationViewModel
    {
        public Guid Id { get; set; }
        public Guid CreateBy { get; set; }
        public Guid RetreatId { get; set; }
        public string? ParticipantEmail { get; set; }
        public string? RetreatName { get; set; }
        public string? RetreatStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalParticipants { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPaid { get; set; }
    }
}
