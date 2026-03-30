using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("entregas")]
public class Submission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int? EvaluacionId { get; set; }

    [Column("estudiante_id")]
    public string? EstudianteId { get; set; }

    [Column("contenido")]
    public string? Contenido { get; set; }

    [Column("archivo_url")]
    public string? ArchivoUrl { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = "enviado";

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey(nameof(EvaluacionId))]
    public Evaluation? Evaluation { get; set; }

    [ForeignKey(nameof(EstudianteId))]
    public User? Student { get; set; }
}
