namespace Business.DTOs.Requests;

public class UpdateCourseDto : CreateCourseDto
{
    public int Id { get; set; }
    public bool IsPublished { get; set; }
}