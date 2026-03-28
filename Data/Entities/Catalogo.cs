using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Catalogo
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public short EntityStatus { get; set; } = 1;

    // Relación: Un catálogo tiene muchas categorías, que a su vez tienen cursos
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
}
