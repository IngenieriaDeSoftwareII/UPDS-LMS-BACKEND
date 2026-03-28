using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Curso
{
    public int Id { get; set; }
    [Required, MaxLength(150)]
    public string Titulo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int? DocenteId { get; set; }
    [MaxLength(50)]
    public string? Nivel { get; set; }
    public int? CategoriaId { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Publicado { get; set; } = false;
    public int DuracionTotalMin { get; set; } = 0;
    public int? MaxEstudiantes { get; set; }
    public short EntityStatus { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public Docente? Docente { get; set; }
    public Categoria? Categoria { get; set; }
    
    public ICollection<Modulo> Modulos { get; set; } = new List<Modulo>();
}
