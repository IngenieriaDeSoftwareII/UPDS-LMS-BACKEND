using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("docentes")]
public class Teacher
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public string UsuarioId { get; set; } = null!;

    [Column("especialidad")]
    public string? Especialidad { get; set; }

    [Column("biografia")]
    public string? Biografia { get; set; }

    [Column("total_cursos")]
    public int TotalCursos { get; set; } = 0;

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UsuarioId))]
    public User Usuario { get; set; } = null!;

    public ICollection<Course> Cursos { get; set; } = [];
}