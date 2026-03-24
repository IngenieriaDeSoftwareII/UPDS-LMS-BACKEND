namespace Data.Entities;

public class Content
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string Type { get; set; } = null!;

    public string? Title { get; set; }

    public string? Url { get; set; }

    public int Order { get; set; } = 1;

    public int Duration { get; set; }

    public short EntityStatus { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime? DeletedAt { get; set; }

    // relacion con leccion
    public Lesson Lesson { get; set; } = null!;
    // relaciones con tipos de contenido
    public VideoContent? Video { get; set; }

    public DocumentContent? Document { get; set; }

    public ImageContent? Image { get; set; }
}