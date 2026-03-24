using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Docente
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = null!;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
    public int TotalCursos { get; set; } = 0;
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UsuarioId))]
    public User User { get; set; } = null!;
    
    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
