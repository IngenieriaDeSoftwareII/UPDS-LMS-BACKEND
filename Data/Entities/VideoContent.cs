using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("contenidos_video")]
public class VideoContent
{
    [Key, ForeignKey("Contenido")]
    [Column("contenido_id")]
    public int ContenidoId { get; set; }

    [Required]
    [Column("url_video")]
    public string UrlVideo { get; set; } = null!;

    [Required]
    [Column("duracion_seg")]
    public int DuracionSeg { get; set; }

    // relacion con contenido
    public Content Contenido { get; set; } = null!;
}