using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("contenidos_imagen")]
public class ImageContent
{
    [Key, ForeignKey("Contenido")]
    [Column("contenido_id")]
    public int ContenidoId { get; set; }

    [Required]
    [Column("url_imagen")]
    public string UrlImagen { get; set; } = null!;

    [Required]
    [Column("formato")]
    public FormatImage Formato { get; set; }

    [Column("ancho_px")]
    public int? AnchoPx { get; set; }

    [Column("alto_px")]
    public int? AltoPx { get; set; }

    [Required]
    [Column("texto_alternativo")]
    [MaxLength(255)]
    public string TextoAlternativo { get; set; } = null!;

    [Column("tamano_kb")]
    public int? TamanoKb { get; set; }

    // relacion con contenido
    public Content Contenido { get; set; } = null!;
}