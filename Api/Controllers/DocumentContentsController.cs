using Business.DTOs.Requests;
using Business.UseCases.DocumentContent;
using Business.UseCases.UpdateDocumentContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentContentsController : ControllerBase
{
    private readonly CreateDocumentContentUseCase _createDocument;
    private readonly ListDocumentContentsUseCase _listDocuments;
    private readonly UpdateDocumentContentUseCase _updateDocument;
    private readonly DeleteDocumentContentUseCase _deleteDocument;
    private readonly UploadDocumentContentUseCase _uploadDocument;
    private readonly GetDocumentSasUrlUseCase _getDocumentSas;

    public DocumentContentsController(
        CreateDocumentContentUseCase createDocument,
        ListDocumentContentsUseCase listDocuments,
        UpdateDocumentContentUseCase updateDocument,
        DeleteDocumentContentUseCase deleteDocument,
        UploadDocumentContentUseCase uploadDocument,
        GetDocumentSasUrlUseCase getDocumentSas)
    {
        _createDocument = createDocument;
        _listDocuments = listDocuments;
        _updateDocument = updateDocument;
        _deleteDocument = deleteDocument;
        _uploadDocument = uploadDocument;
        _getDocumentSas = getDocumentSas;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateDocumentContentDto dto)
    {
        var result = await _createDocument.ExecuteAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _listDocuments.ExecuteAsync();
        return Ok(result);
    }

    [HttpGet("{contentId}")]
    public async Task<IActionResult> GetById(int contentId)
    {
        var document = (await _listDocuments.ExecuteAsync())
            .FirstOrDefault(d => d.ContentId == contentId);

        if (document == null)
            return NotFound();

        return Ok(document);
    }

    [HttpPut("Update/{contentId}")]
    public async Task<IActionResult> Update(int contentId, [FromBody] UpdateDocumentContentDto dto)
    {
        var success = await _updateDocument.ExecuteAsync(contentId, dto);
        if (!success)
            return NotFound("Documento no encontrado");

        return Ok("Documento actualizado correctamente");
    }

    [HttpPut("UpdateFile/{contentId}")]
    public async Task<IActionResult> UpdateFile(int contentId, [FromForm] UploadFileDto dto)
    {
        if (dto == null)
            return BadRequest("Datos inválidos");

        var requestDto = new UpdateDocumentContentDto
        {
            Title = dto.Title ?? dto.File?.FileName ?? "Sin título",
            Order = dto.Order,
            PageCount = dto.PageCount,
            File = dto.File // ⚡ aquí se pasa el archivo
        };

        try
        {
            var updated = await _updateDocument.ExecuteAsync(contentId, requestDto);
            if (!updated)
                return NotFound("Documento no encontrado");

            return Ok(new { Message = "Documento actualizado correctamente ✅" });
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, "Error al actualizar documento: " + msg);
        }
    }

    [HttpDelete("Delete/{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var result = await _deleteDocument.ExecuteAsync(contentId);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = dto.File.OpenReadStream();
        var requestDto = new UploadDocumentContentRequestDto
        {
            FileStream = stream,
            FileName = dto.File.FileName,
            LessonId = dto.LessonId,
            Title = dto.Title ?? dto.File.FileName,
            Format = dto.Format ?? Path.GetExtension(dto.File.FileName).TrimStart('.'),
            SizeKb = (int)(dto.File.Length / 1024),
            PageCount = dto.PageCount,
            Order = dto.Order
        };

        try
        {
            var result = await _uploadDocument.ExecuteAsync(requestDto);
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

    [HttpGet("GetSasUrl/{contentId}")]
    public async Task<IActionResult> GetSasUrl(int contentId)
    {
        var document = (await _listDocuments.ExecuteAsync())
            .FirstOrDefault(d => d.ContentId == contentId);

        if (document == null)
            return NotFound("Documento no encontrado");

        try
        {
            var containerName = "documents";
            var blobName = document.FileUrl;

            var sasUri = await _getDocumentSas.ExecuteAsync(
                blobName,
                containerName,
                TimeSpan.FromHours(1)
            );

            return Ok(new { url = sasUri.ToString() });
        }
        catch (Exception ex)
        {
            return BadRequest("Error generando SAS: " + ex.Message);
        }
    }
}