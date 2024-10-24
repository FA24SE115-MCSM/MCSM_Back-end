﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

[Table("RetreatRegistration")]
public partial class RetreatRegistration
{
    [Key]
    public Guid Id { get; set; }

    public Guid CreateBy { get; set; }

    public Guid RetreatId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdateAt { get; set; }

    [Column(TypeName = "decimal(16, 2)")]
    public decimal TotalCost { get; set; }

    public int TotalParticipants { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsPaid { get; set; }

    [ForeignKey("CreateBy")]
    [InverseProperty("RetreatRegistrations")]
    public virtual Account CreateByNavigation { get; set; }

    [InverseProperty("RetreatReg")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("RetreatId")]
    [InverseProperty("RetreatRegistrations")]
    public virtual Retreat Retreat { get; set; }

    [InverseProperty("RetreatReg")]
    public virtual ICollection<RetreatRegistrationParticipant> RetreatRegistrationParticipants { get; set; } = new List<RetreatRegistrationParticipant>();
}