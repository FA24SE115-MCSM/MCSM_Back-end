﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

[Table("ToolOperation")]
public partial class ToolOperation
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool IsIncrement { get; set; }

    [InverseProperty("ToolOp")]
    public virtual ICollection<ToolHistory> ToolHistories { get; set; } = new List<ToolHistory>();
}