using System.IO;

namespace Business.DTOs.Requests;

public class UploadDocumentContentRequestDto
{
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public int LessonId { get; set; }
    public string Title { get; set; } = null!;

    public int Order {get; set;}
    public string Format { get; set; } = null!;
    public int? SizeKb { get; set; }
    public int? PageCount { get; set; }
}