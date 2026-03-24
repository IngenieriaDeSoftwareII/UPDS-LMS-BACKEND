using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentContentsController(
    CreateDocumentContentUseCase createDocument,
    ListDocumentContentsUseCase listDocuments,
    UpdateDocumentContentUseCase updateDocument,
    DeleteDocumentContentUseCase deleteDocument) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateDocumentContentDto dto)
    {
        var result = await createDocument.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await listDocuments.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("{contentId}")]
    public async Task<IActionResult> Update(int contentId, CreateDocumentContentDto dto)
    {
        var result = await updateDocument.ExecuteAsync(contentId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var result = await deleteDocument.ExecuteAsync(contentId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}