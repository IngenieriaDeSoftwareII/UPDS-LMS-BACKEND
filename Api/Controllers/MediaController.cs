using Business.UseCases.UpdateDocumentContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController(UploadImageUseCase uploadImage) : ControllerBase
{
    [HttpPost("images")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No se recibió ningún archivo.");

        await using var stream = file.OpenReadStream();
        var url = await uploadImage.ExecuteAsync(stream, file.FileName);

        return Ok(new { url });
    }
}
