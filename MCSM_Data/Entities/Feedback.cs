﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

[Table("Feedback")]
public partial class Feedback
{
    [Key]
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid RetreatId { get; set; }

    [Required]
    public string Content { get; set; }

    public int Rating { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Feedbacks")]
    public virtual Account CreatedByNavigation { get; set; }

    [ForeignKey("RetreatId")]
    [InverseProperty("Feedbacks")]
    public virtual Retreat Retreat { get; set; }
}