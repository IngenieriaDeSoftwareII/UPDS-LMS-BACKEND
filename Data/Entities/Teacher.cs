using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string? Specialty { get; set; }
    public string? Biography { get; set; }
    public int TotalCourses { get; set; } = 0;
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}