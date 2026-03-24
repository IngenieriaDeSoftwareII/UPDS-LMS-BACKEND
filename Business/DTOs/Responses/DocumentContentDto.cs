namespace Business.DTOs.Responses;

public class DocumentContentDto
{
    public int ContentId { get; set; }
    public string FileUrl { get; set; } = null!;
    public string Format { get; set; } = null!;
    public int? SizeKb { get; set; }
    public int? PageCount { get; set; }
}