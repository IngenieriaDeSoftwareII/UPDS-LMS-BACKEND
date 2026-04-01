namespace Business.DTOs.Requests;

public class UpdateImageContentDto
{
    public string AltText { get; set; } = null!;
    public int? Order { get; set; }
}