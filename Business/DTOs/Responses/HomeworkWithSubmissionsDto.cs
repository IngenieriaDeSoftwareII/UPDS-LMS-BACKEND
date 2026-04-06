using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class HomeworkWithSubmissionsDto
{
    public HomeworkDto Homework { get; set; } = null!;
    public IEnumerable<HomeworkSubmissionDto> Submissions { get; set; } = null!;
}