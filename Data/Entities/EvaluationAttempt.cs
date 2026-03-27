using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("intentos_evaluacion")]
public class EvaluationAttempt
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int EvaluacionId { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("numero_intento")]
    public int NumeroIntento { get; set; } = 1;

    [Column("puntaje_obtenido", TypeName = "decimal(5,2)")]
    public decimal PuntajeObtenido { get; set; }

    [Column("es_aprobado")]
    public bool EsAprobado { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public Evaluation Evaluaciones { get; set; } = null!;
    public ICollection<EvaluationAnswer> Respuestas { get; set; } = [];
}

