using Business.DTOs.Requests;
using Business.UseCases.VideoContent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoContentsController(
    CreateVideoContentUseCase createVideo,
    ListVideoContentsUseCase listVideos,
    UpdateVideoContentUseCase updateVideo,
    DeleteVideoContentUseCase deleteVideo) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateVideoContentDto dto)
    {
        var result = await createVideo.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listVideos.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{contentId}")]
    public async Task<IActionResult> Update(int contentId, CreateVideoContentDto dto)
    {
        var result = await updateVideo.ExecuteAsync(contentId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var result = await deleteVideo.ExecuteAsync(contentId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}