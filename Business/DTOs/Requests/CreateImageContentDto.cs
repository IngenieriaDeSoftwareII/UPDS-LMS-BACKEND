namespace Business.DTOs.Requests;

public class CreateImageContentDto
{
    public string ImageUrl { get; set; } = null!;
    public string Format { get; set; } = null!;
    public int? WidthPx { get; set; }
    public int? HeightPx { get; set; }
    public string AltText { get; set; } = null!;
    public int? SizeKb { get; set; }
}