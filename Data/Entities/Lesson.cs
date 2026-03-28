using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("lecciones")]
public class Lesson
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("curso_id")]
    public int? CursoId { get; set; }

    [Required]
    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Required]
    [Column("orden")]
    public int Orden { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // Relaciones con contenidos
    public ICollection<Content> Contenidos { get; set; } = [];
}