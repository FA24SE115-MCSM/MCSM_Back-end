﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

[Table("RetreatLesson")]
public partial class RetreatLesson
{
    [Key]
    public Guid Id { get; set; }

    public Guid RetreatId { get; set; }

    public Guid LessonId { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("RetreatLessons")]
    public virtual Lesson Lesson { get; set; }

    [ForeignKey("RetreatId")]
    [InverseProperty("RetreatLessons")]
    public virtual Retreat Retreat { get; set; }

    [InverseProperty("RetreatLesson")]
    public virtual ICollection<RetreatSchedule> RetreatSchedules { get; set; } = new List<RetreatSchedule>();
}