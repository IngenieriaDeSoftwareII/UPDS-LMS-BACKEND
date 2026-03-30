using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("contenidos")]
public class Content
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("leccion_id")]
    public int LeccionId { get; set; }

    [Required]
    [Column("tipo")]
    public TypeContent Tipo { get; set; }

    [Required]
    [Column("titulo")]
    [MaxLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("url")]
    public string? Url { get; set; }

    [Required]
    [Column("orden")]
    public int Orden { get; set; } = 1;

    [Required]
    [Column("duracion")]
    public int Duracion { get; set; }

    [Column("entity_status")]
    public short EntityStatus { get; set; } = 1;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // relacion con leccion
    public Lesson Leccion { get; set; } = null!;
    // relaciones con tipos de contenido
    public VideoContent? Video { get; set; }

    public DocumentContent? Documento { get; set; }

    public ImageContent? Imagen { get; set; }
}