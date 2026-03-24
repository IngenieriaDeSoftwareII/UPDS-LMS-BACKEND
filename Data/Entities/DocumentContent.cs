using Data.Enums;

namespace Data.Entities;

public class DocumentContent
{
    public int ContentId { get; set; }

    public string FileUrl { get; set; } = null!;

    public FormatDocument Format { get; set; }

    public int? SizeKb { get; set; }

    public int? PageCount { get; set; }

    // relacion con contenido
    public Content Content { get; set; } = null!;
}
