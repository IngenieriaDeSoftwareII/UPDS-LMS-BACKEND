using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("categorias")]
public class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("catalogo_id")]
    public int? CatalogoId { get; set; }

    [ForeignKey(nameof(CatalogoId))]
    public Catalog? Catalogo { get; set; }

    public ICollection<Course> Cursos { get; set; } = [];
}