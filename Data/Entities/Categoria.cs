using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Categoria
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = null!;
    [Required, MaxLength(100)]
    public string Slug { get; set; } = null!;
    public string? Descripcion { get; set; }
    public short EntityStatus { get; set; } = 1;

    public int? CatalogoId { get; set; }
    public Catalogo? Catalogo { get; set; }

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
