using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("opciones_respuesta")]
public class AnswerOption
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("pregunta_id")]
    public int PreguntaId { get; set; }

    [Column("texto")]
    public string Texto { get; set; } = null!;

    [Column("es_correcta")]
    public bool EsCorrecta { get; set; }

    [Column("orden")]
    public int Orden { get; set; } = 1;

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    public Question Preguntas { get; set; } = null!;
    public ICollection<EvaluationAnswer> RespuestasSeleccionadas { get; set; } = [];
}

