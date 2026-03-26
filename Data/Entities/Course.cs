using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("cursos")]
public class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("docente_id")]
    public int? DocenteId { get; set; }

    [Column("nivel")]
    [MaxLength(50)]
    public string? Nivel { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [Column("imagen_url")]
    public string? ImagenUrl { get; set; }

    [Column("publicado")]
    public bool Publicado { get; set; }

    [Column("duracion_total_min")]
    public int DuracionTotalMin { get; set; }

    [Column("max_estudiantes")]
    public int? MaxEstudiantes { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public ICollection<Inscription> Inscripciones { get; set; } = [];

    public ICollection<Module> Modulos { get; set; } = [];
}

