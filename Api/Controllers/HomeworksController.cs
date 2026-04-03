using Business.DTOs.Requests;
using Business.UseCases.Homework;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HomeworksController(
    CreateHomeworkUseCase createHomework,
    UpdateHomeworkUseCase updateHomework,
    DeleteHomeworkUseCase deleteHomework,
    ListHomeworkUseCase listHomework,
    IHomeworkRepository repository,
    IStorageService storageService
) : ControllerBase
{
    [HttpPost("Create")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> Create([FromForm] CreateHomeworkDto dto)
    {
        var result = await createHomework.ExecuteAsync(dto);

        if (!result.IsSuccess)
        {
            Console.WriteLine("❌ ERROR:");
            foreach (var e in result.Errors)
                Console.WriteLine(e);

            return BadRequest(result.Errors);
        }

        Console.WriteLine("✅ OK");

        return Ok(result.Value);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listHomework.ExecuteAsync();
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateHomeworkDto dto)
    {
        dto.Id = id;

        var result = await updateHomework.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> Delete(int id)
    {
        var homework = await repository.GetByIdAsync(id);
        if (homework == null)
            return NotFound();

        if (!string.IsNullOrEmpty(homework.UrlArchivo))
        {
            await storageService.DeleteFileAsync(homework.UrlArchivo, "homeworks");
        }

        await deleteHomework.ExecuteAsync(id);

        return Ok(true);
    }


    [HttpGet("GetSasUrl/{id}")]
    public async Task<IActionResult> GetSasUrl(int id)
    {
        var homework = await repository.GetByIdAsync(id);

        if (homework == null)
            return NotFound("No existe homework");

        if (string.IsNullOrEmpty(homework.UrlArchivo))
            return BadRequest("No hay archivo");

        var url = await storageService.GetReadUrlAsync(
            homework.UrlArchivo,
            "homeworks",
            TimeSpan.FromMinutes(30)
        );

        return Ok(new { url = url.ToString() });
    }
}