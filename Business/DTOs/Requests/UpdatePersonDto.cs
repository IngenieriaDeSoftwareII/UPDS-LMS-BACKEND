using Data.Enums;

namespace Business.DTOs.Requests;

public class UpdatePersonDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MotherLastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? NationalId { get; set; }
    public string? NationalIdExpedition { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
