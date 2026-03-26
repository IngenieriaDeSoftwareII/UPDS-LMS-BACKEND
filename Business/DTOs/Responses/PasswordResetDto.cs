namespace Business.DTOs.Responses;

public class PasswordResetDto
{
    public string UserId { get; set; } = null!;
    public string TemporaryPassword { get; set; } = null!;
}
