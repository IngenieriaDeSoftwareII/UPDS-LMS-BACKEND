using Business.DTOs.Requests;
using Business.UseCases;
using Business.UseCases.DocumentContent;
using Business.UseCases.UpdateDocumentContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentContentsController(
    CreateDocumentContentUseCase createDocument,
    ListDocumentContentsUseCase listDocuments,
    UpdateDocumentContentUseCase updateDocument,
    DeleteDocumentContentUseCase deleteDocument,
    UploadDocumentContentUseCase uploadDocument) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateDocumentContentDto dto)
    {
        var result = await createDocument.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listDocuments.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{contentId}")]
    public async Task<IActionResult> Update(int contentId, CreateDocumentContentDto dto)
    {
        var result = await updateDocument.ExecuteAsync(contentId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var result = await deleteDocument.ExecuteAsync(contentId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using var stream = dto.File.OpenReadStream();
        var requestDto = new UploadDocumentContentRequestDto
        {
            FileStream = stream,
            FileName = dto.File.FileName,
            LessonId = dto.LessonId,
            Title = dto.Title ?? Path.GetFileNameWithoutExtension(dto.File.FileName),//extraer el nombre del archivo sin la extension
            Format = dto.Format ?? Path.GetExtension(dto.File.FileName).TrimStart('.'),//extraer lam extension y borrar el punto
            SizeKb = dto.SizeKb,
            PageCount = dto.PageCount
        };

        try
        {
            var result = await uploadDocument.ExecuteAsync(requestDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}