using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Module
{
    public int Id { get; set; }
    [Required, MaxLength(150)]
    public string Title { get; set; } = null!;
    public int CourseId { get; set; }
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Course? Course { get; set; }
}