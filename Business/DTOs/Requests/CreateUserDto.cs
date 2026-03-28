namespace Business.DTOs.Requests;

public class CreateUserDto
{
    public int PersonId { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}
