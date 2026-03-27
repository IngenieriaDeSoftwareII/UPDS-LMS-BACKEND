namespace Business.DTOs.Responses;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long SessionTTL { get; set; }
    public string Role { get; set; } = null!;
    public string RedirectTo { get; set; } = null!;
}