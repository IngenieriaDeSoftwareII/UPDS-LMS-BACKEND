using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

[Table("tareas")]
public class Homework
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // Relación con lección
    [Column("leccion_id")]
    public int? LessonId { get; set; }

    [Required]
    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    // Fecha en la que se habilita la tarea
    [Column("fecha_apertura")]
    public DateTime FechaApertura { get; set; }

    // Fecha límite de entrega
    [Column("fecha_entrega")]
    public DateTime FechaEntrega { get; set; }

    // Fecha máxima (con retraso)
    [Column("fecha_limite")]
    public DateTime? FechaLimite { get; set; }
    
    [Column("url_archivo")]
    public string? UrlArchivo { get; set; }

    [Column("formato")]
    public FormatDocument? Formato { get; set; }

    [Column("tamano_kb")]
    public int? TamanoKb { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [Column("docente_id")]
    public int DocenteId { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = "activo";

    [Column("permite_entrega_tardia")]
    public bool PermiteEntregaTardia { get; set; } = false;

    // Relación
    public Lesson? Lesson { get; set; }

    public ICollection<HomeworkSubmission> Submissions { get; set; } = [];
}