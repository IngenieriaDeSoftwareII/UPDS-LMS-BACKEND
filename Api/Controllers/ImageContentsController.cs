using Business.DTOs.Requests;
using Business.UseCases.ImageContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageContentsController(
    CreateImageContentUseCase createImage,
    ListImageContentsUseCase listImages,
    ListImageContentsByCourseUseCase listImagesByCourse,
    UpdateImageContentUseCase updateImage,
    UploadImageContentUseCase uploadImage,
    DeleteImageContentUseCase deleteImage) : ControllerBase
{
    private readonly ListImageContentsByCourseUseCase _listImagesByCourse = listImagesByCourse;
    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(
        [FromForm] int lessonId,
        [FromForm] string? title,
        [FromForm] int? order,
        [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Archivo no proporcionado.");

        await using var stream = file.OpenReadStream();

        var result = await uploadImage.ExecuteAsync(
            lessonId,
            title ?? file.FileName,
            stream,
            file.FileName,
            order ?? 1 
        );

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateImageContentDto dto)
    {
        var result = await createImage.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listImages.ExecuteAsync();
        return Ok(result);
    }

    [HttpGet("GetByCourse/{courseId}")]
    public async Task<IActionResult> GetByCourse(int courseId)
    {
        var result = await _listImagesByCourse.ExecuteAsync(courseId);
        return Ok(result);
    }

    [HttpPut("Update/{contentId}")]
    public async Task<IActionResult> Update(
        int contentId,
        [FromForm] string altText,
        [FromForm] int? order,
        [FromForm] int? lessonId,
        [FromForm] IFormFile? file
    )
    {
        try
        {
            Stream? stream = null;
            string? fileName = null;
            long? fileSize = null;

            if (file != null && file.Length > 0)
            {
                stream = file.OpenReadStream();
                fileName = file.FileName;
                fileSize = file.Length;
            }

            var dto = new UpdateImageContentDto
            {
                AltText = altText,
                Order = order,
                LessonId = lessonId
            };

            var result = await updateImage.ExecuteAsync(
                contentId,
                dto,
                stream,
                fileName,
                fileSize
            );

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                error = "Error interno del servidor"
            });
        }
    }

    [HttpDelete("Delete/{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var result = await deleteImage.ExecuteAsync(contentId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}