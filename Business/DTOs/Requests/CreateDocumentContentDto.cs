namespace Business.DTOs.Requests;

public class CreateDocumentContentDto
{
    public int ContentId { get; set; }
    public string FileUrl { get; set; } = null!;
    public string Format { get; set; } = null!;
    public int? SizeKb { get; set; }
    public int? PageCount { get; set; }
}