using Data.Enums;

namespace Business.DTOs.Requests;

public class CreatePersonDto
{
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