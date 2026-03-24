namespace Data.Entities;

public class ImageContent
{
    public int ContentId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string Format { get; set; } = null!;

    public int? WidthPx { get; set; }

    public int? HeightPx { get; set; }

    public string AltText { get; set; } = null!;

    public int? SizeKb { get; set; }

    // relacion con contenido
    public Content Content { get; set; } = null!;
}