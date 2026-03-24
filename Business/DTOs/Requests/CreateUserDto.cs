namespace Business.DTOs.Requests;

public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int PersonId { get; set; }
}