using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("nota_final_modulo")]
public class ModuleFinalGrade
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("estudiante_id")]
    public string EstudianteId { get; set; } = null!;

    [Column("modulo_id")]
    public int ModuloId { get; set; }

    [Column("nota_ponderada", TypeName = "decimal(5,2)")]
    public decimal? NotaPonderada { get; set; }

    [Column("items_calificados")]
    public int ItemsCalificados { get; set; } = 0;

    [Column("items_totales")]
    public int ItemsTotales { get; set; } = 0;

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = "en_progreso";

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(EstudianteId))]
    public User Student { get; set; } = null!;

    [ForeignKey(nameof(ModuloId))]
    public Module Module { get; set; } = null!;
}
