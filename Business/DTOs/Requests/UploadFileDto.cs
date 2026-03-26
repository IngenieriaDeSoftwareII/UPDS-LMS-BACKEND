using Microsoft.AspNetCore.Http;

public class UploadFileDto
{
    public IFormFile? File { get; set; }
    public int LessonId { get; set; }
    public string? Title{ get; set; }
    public string? Format{ get; set; }
    public int? SizeKb { get; set; }
    public int? PageCount { get; set; }
}