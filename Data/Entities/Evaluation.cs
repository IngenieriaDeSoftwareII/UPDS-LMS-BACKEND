using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("evaluaciones")]
public class Evaluation
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

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = null!;

    [Column("puntaje_maximo", TypeName = "decimal(5,2)")]
    public decimal? PuntajeMaximo { get; set; }

    [Column("puntaje_minimo_aprobacion", TypeName = "decimal(5,2)")]
    public decimal? PuntajeMinimoAprobacion { get; set; }

    [Column("intentos_permitidos")]
    public int IntentosPermitidos { get; set; } = 1;

    [Column("tiempo_limite_max")]
    public int? TiempoLimiteMax { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public Course Cursos { get; set; } = null!;

    public ICollection<Question> Preguntas { get; set; } = [];
    public ICollection<EvaluationAttempt> Intentos { get; set; } = [];
}

