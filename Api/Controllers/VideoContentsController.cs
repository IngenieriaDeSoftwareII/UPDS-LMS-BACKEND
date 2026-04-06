using Business.UseCases.VideoContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoContentsController(
    UploadVideoContentUseCase upload,
    ListVideoContentsUseCase list,
    UpdateVideoContentUseCase update,
    DeleteVideoContentUseCase delete
) : ControllerBase
{
    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(
        [FromForm] int lessonId,
        [FromForm] string title,
        [FromForm] int order,
        [FromForm] int duracionSeg,
        [FromForm] IFormFile file)
    {
        var stream = file.OpenReadStream();

        var result = await upload.ExecuteAsync(
            lessonId,
            title,
            stream,
            file.FileName,
            order,
            duracionSeg
        );

        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await list.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromForm] int? duracionSeg,
        [FromForm] int? lessonId,
        [FromForm] int? order,
        [FromForm] IFormFile? file)
    {
        Stream? stream = file?.OpenReadStream();

        var dto = new Business.DTOs.Requests.UpdateVideoContentDto
        {
            DuracionSeg = duracionSeg,
            LessonId = lessonId,
            Order = order
        };

        var result = await update.ExecuteAsync(id, dto, stream, file?.FileName);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await delete.ExecuteAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}