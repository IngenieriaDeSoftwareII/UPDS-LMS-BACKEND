using Business.DTOs.Requests;
using Business.UseCases.ImageContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageContentsController(
    CreateImageContentUseCase createImage,
    ListImageContentsUseCase listImages,
    UpdateImageContentUseCase updateImage,
    UploadImageContentUseCase uploadImage,
    DeleteImageContentUseCase deleteImage) : ControllerBase
{
    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromForm] int lessonId, [FromForm] string? title, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Archivo no proporcionado.");

        await using var stream = file.OpenReadStream();
        var result = await uploadImage.ExecuteAsync(lessonId, title ?? file.FileName, stream, file.FileName);

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

    [HttpPut("Update/{contentId}")]
    public async Task<IActionResult> Update(int contentId, CreateImageContentDto dto)
    {
        var result = await updateImage.ExecuteAsync(contentId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
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