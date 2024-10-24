﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

[Table("RetreatGroup")]
public partial class RetreatGroup
{
    [Key]
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public Guid MonkId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [ForeignKey("MonkId")]
    [InverseProperty("RetreatGroups")]
    public virtual Account Monk { get; set; }

    [ForeignKey("RetreatId")]
    [InverseProperty("RetreatGroups")]
    public virtual Retreat Retreat { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<RetreatGroupMember> RetreatGroupMembers { get; set; } = new List<RetreatGroupMember>();

    [InverseProperty("Group")]
    public virtual ICollection<RetreatGroupMessage> RetreatGroupMessages { get; set; } = new List<RetreatGroupMessage>();

    [InverseProperty("Group")]
    public virtual ICollection<RetreatSchedule> RetreatSchedules { get; set; } = new List<RetreatSchedule>();
}