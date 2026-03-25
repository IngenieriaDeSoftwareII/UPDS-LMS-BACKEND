using Business.DTOs.Requests;
using Business.UseCases.Content;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentsController(
    CreateContentUseCase createContent,
    ListContentsUseCase listContents,
    UpdateContentUseCase updateContent,
    DeleteContentUseCase deleteContent) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateContentDto dto)
    {
        var result = await createContent.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }



    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listContents.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateContentDto dto)
    {
        var result = await updateContent.ExecuteAsync(id, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteContent.ExecuteAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}