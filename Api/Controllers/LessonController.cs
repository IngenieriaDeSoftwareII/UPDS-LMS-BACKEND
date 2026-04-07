using Business.DTOs.Requests;
using Business.UseCases.Lesson;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController(
    CreateLessonUseCase createLesson,
    ListLessonsUseCase listLessons,
    UpdateLessonUseCase updateLesson,
    DeleteLessonUseCase deleteLesson) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateLessonDto dto)
    {
        var result = await createLesson.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listLessons.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, CreateLessonDto dto)
    {
        var result = await updateLesson.ExecuteAsync(id, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteLesson.ExecuteAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}