using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Catalog
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short EntityStatus { get; set; } = 1;

    public ICollection<Category> Categorias { get; set; } = new List<Category>();
}
