using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("preguntas")]
public class Question
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int EvaluacionId { get; set; }

    [Column("enunciado")]
    public string Enunciado { get; set; } = null!;

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = null!;

    [Column("puntos")]
    public int Puntos { get; set; } = 1;

    [Column("orden")]
    public int Orden { get; set; } = 1;

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public Evaluation Evaluaciones { get; set; } = null!;

    public ICollection<AnswerOption> OpcionesRespuesta { get; set; } = [];
    public ICollection<EvaluationAnswer> Respuestas { get; set; } = [];
}

