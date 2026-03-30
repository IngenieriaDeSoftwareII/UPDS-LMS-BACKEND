using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("modulos")]
public class Module
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("curso_id")]
    public int CursoId { get; set; }

    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("orden")]
    public int Orden { get; set; }

    [Column("entity_status")]
    public short? EntityStatus { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }


    [ForeignKey(nameof(CursoId))]
    public Course? Cursos { get; set; }
  
    public ICollection<Lesson> Lecciones { get; set; } = [];

    public ICollection<GradableItem> ItemsCalificables { get; set; } = [];
}