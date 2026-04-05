namespace Business.DTOs.Responses;

public class VideoContentDto
{
    public int ContentId { get; set; }
    public string UrlVideo { get; set; } = null!;
    public int DuracionSeg { get; set; }

    public ContentDto? Content { get; set; }
}