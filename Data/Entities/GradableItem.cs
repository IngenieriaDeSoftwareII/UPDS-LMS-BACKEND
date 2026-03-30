using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("items_calificables")]
public class GradableItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("modulo_id")]
    public int ModuloId { get; set; }

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = null!;

    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("evaluacion_id")]
    public int? EvaluacionId { get; set; }

    [Column("leccion_id")]
    public int? LeccionId { get; set; }

    [Column("ponderacion", TypeName = "decimal(5,2)")]
    public decimal Ponderacion { get; set; }

    [Column("orden")]
    public int Orden { get; set; } = 1;

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(ModuloId))]
    public Module Module { get; set; } = null!;

    [ForeignKey(nameof(EvaluacionId))]
    public Evaluation? Evaluation { get; set; }

    [ForeignKey(nameof(LeccionId))]
    public Lesson? Lesson { get; set; }
}
