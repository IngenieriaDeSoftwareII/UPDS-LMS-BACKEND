namespace Business.DTOs.Requests;

public class ChangeUserStatusDto
{
    public bool IsActive { get; set; }
    public DateTimeOffset? LockedUntil { get; set; }
}
