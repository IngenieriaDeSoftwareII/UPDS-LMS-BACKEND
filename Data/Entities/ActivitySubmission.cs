using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("actividad_entregas")]
public class ActivitySubmission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public string UsuarioId { get; set; } = null!;

    [Column("leccion_id")]
    public int LeccionId { get; set; }

    [Column("url_archivo")]
    public string UrlArchivo { get; set; } = null!;

    [Column("comentario")]
    public string? Comentario { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = "enviado";

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(UsuarioId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(LeccionId))]
    public Lesson Lesson { get; set; } = null!;
}
