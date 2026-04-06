using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

[Table("entregas_tareas")]
public class HomeworkSubmission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tarea_id")]
    public int HomeworkId { get; set; }

    [Required]
    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("url_archivo")]
    public string? UrlArchivo { get; set; }

    [Column("formato")]
    public FormatDocument? Formato { get; set; }

    [Column("tamano_kb")]
    public int? TamanoKb { get; set; }

    [Column("comentario")]
    public string? Comentario { get; set; }

    [Column("Revisado")]
    public bool Revisado { get; set; } = false;
    
    [Column("fecha_entrega")]
    public DateTime FechaEntrega { get; set; } = DateTime.Now;

    [Column("feedback")]
    public string? Feedback { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = "pendiente";// pendiente, entregado, tarde, calificado

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public Homework Homework { get; set; } = null!;
    public Person Usuario { get; set; } = null!;
}