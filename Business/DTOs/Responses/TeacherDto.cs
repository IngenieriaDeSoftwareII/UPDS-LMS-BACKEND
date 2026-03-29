using System;
using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class TeacherDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string? Biography { get; set; }
    public int TotalCourses { get; set; }
    public DateTime CreatedAt { get; set; }

    public string UserFullName { get; set; } = string.Empty; // Mapped from Person

    public IEnumerable<CourseDto> Courses { get; set; } = new List<CourseDto>();
}