using System;
using System.Collections.Generic;

namespace MCSM_Data.Entities;

public partial class RetreatRegistration
{
    public Guid Id { get; set; }

    public Guid CreateBy { get; set; }

    public Guid RetreatId { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public decimal TotalCost { get; set; }

    public int TotalParticipants { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsPaid { get; set; }

    public virtual Account CreateByNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Retreat Retreat { get; set; } = null!;

    public virtual ICollection<RetreatRegistrationParticipant> RetreatRegistrationParticipants { get; set; } = new List<RetreatRegistrationParticipant>();
}
