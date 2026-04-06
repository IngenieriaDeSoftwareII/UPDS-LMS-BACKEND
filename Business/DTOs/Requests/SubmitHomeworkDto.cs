using Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Requests;

public class SubmitHomeworkDto
{
    public int HomeworkId { get; set; }

    public string? UrlArchivo { get; set; }

    public IFormFile? File { get; set; }

    public FormatDocument? Formato { get; set; }

    public int? TamanoKb { get; set; }

    public string? Comentario { get; set; }
}