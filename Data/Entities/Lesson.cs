namespace Data.Entities;

public class Lesson
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Order { get; set; }

    public short EntityStatus { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime? DeletedAt { get; set; }

    // Relaciones con contenidos
    public ICollection<Content> Contents { get; set; } = [];
}