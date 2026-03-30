using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("calificaciones_items")]
public class ItemGrade
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("estudiante_id")]
    public string EstudianteId { get; set; } = null!;

    [Column("docente_id")]
    public int DocenteId { get; set; }

    [Column("nota", TypeName = "decimal(5,2)")]
    public decimal Nota { get; set; }

    [Column("feedback")]
    public string? Feedback { get; set; }

    [Column("entrega_id")]
    public int? EntregaId { get; set; }

    [Column("actividad_entrega_id")]
    public int? ActividadEntregaId { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(ItemId))]
    public GradableItem Item { get; set; } = null!;

    [ForeignKey(nameof(EstudianteId))]
    public User Student { get; set; } = null!;

    [ForeignKey(nameof(DocenteId))]
    public Teacher Teacher { get; set; } = null!;

    [ForeignKey(nameof(EntregaId))]
    public Submission? Submission { get; set; }

    [ForeignKey(nameof(ActividadEntregaId))]
    public ActivitySubmission? ActivitySubmission { get; set; }
}
