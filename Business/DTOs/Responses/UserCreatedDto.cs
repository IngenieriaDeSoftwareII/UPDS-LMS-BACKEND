namespace Business.DTOs.Responses;

public class UserCreatedDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string TemporaryPassword { get; set; } = null!;
}
