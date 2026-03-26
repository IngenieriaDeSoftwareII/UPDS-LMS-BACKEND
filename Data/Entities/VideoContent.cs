using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class VideoContent
{
    [Key, ForeignKey("Content")]
    public int ContentId { get; set; }

    public string VideoUrl { get; set; } = null!;

    public int DurationSeconds { get; set; }

    // relacion con contenido
    public Content Content { get; set; } = null!;
}