using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Modulo
{
    public int Id { get; set; }
    public int? CursoId { get; set; }
    [Required, MaxLength(150)]
    public string Titulo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public Curso? Curso { get; set; }
}
