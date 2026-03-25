using Data.Enums;

namespace Data.Entities;

public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MotherLastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string NationalId { get; set; } = null!;

    public string NationalIdExpedition { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public User? User { get; set; }

    public ICollection<Inscription> Inscripciones { get; set; } = [];
}
