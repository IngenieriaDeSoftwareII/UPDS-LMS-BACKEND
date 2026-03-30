using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("catalogos")]
public class Catalog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public ICollection<Category> Categorias { get; set; } = [];
}
