using Data.Enums;

namespace Business.DTOs.Responses;

public class ProfileDto
{
    // Identity User Info
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    
    // Exact Person Info
    public int PersonId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MotherLastName { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string NationalId { get; set; } = null!;
    public string NationalIdExpedition { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
