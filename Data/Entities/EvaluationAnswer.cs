using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("respuestas_evaluacion")]
public class EvaluationAnswer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("intento_id")]
    public int IntentoId { get; set; }

    [Column("pregunta_id")]
    public int PreguntaId { get; set; }

    [Column("opcion_respuesta_id")]
    public int? OpcionRespuestaId { get; set; }

    [Column("respuesta_texto")]
    public string? RespuestaTexto { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public EvaluationAttempt Intentos { get; set; } = null!;
    public Question Preguntas { get; set; } = null!;
    public AnswerOption? OpcionesRespuesta { get; set; }
}

