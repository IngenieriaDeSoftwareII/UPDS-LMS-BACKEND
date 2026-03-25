using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("progreso_lecciones")]
public class LessonProgress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("leccion_id")]
    public int LeccionId { get; set; }

    [Column("completado")]
    public bool? Completado { get; set; } = false;

    [Column("posicion_actual", TypeName = "decimal(6,2)")]
    public decimal? PosicionActual { get; set; }

    [Column("fecha_completado")]
    public DateTime? FechaCompletado { get; set; }

    [Column("entity_status")]
    public short? EntityStatus { get; set; } = 1;

    public User User { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}