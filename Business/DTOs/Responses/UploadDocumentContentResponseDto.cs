namespace Business.DTOs.Responses;

public class UploadDocumentContentResponseDto
{
    public int ContentId { get; set; }
    public int DocumentId { get; set; }
    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int LessonId { get; set; }
    public DateTime CreatedAt { get; set; }
}