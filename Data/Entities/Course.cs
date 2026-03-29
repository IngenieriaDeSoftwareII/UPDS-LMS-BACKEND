using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Course
{
    public int Id { get; set; }
    [Required, MaxLength(150)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? TeacherId { get; set; }
    [MaxLength(50)]
    public string? Level { get; set; }
    public int? CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; } = false;
    public int TotalDurationMin { get; set; } = 0;
    public int? MaxStudents { get; set; }
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public Teacher? Teacher { get; set; }
    public Category? Category { get; set; }

    // Asumiendo que Modulo aún no lo cambiaste o lo cambiarás a Module pronto.
    public ICollection<Module> Modules { get; set; } = new List<Module>();
}