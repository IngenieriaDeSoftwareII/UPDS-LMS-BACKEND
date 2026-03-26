using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ImageContent
{
    [Key, ForeignKey("Content")]
    public int ContentId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public FormatImage Format { get; set; }

    public int? WidthPx { get; set; }

    public int? HeightPx { get; set; }

    public string AltText { get; set; } = null!;

    public int? SizeKb { get; set; }

    // relacion con contenido
    public Content Content { get; set; } = null!;

}