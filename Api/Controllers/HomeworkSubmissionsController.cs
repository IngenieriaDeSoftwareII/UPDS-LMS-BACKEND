using System.Security.Claims;
using Business.DTOs.Requests;
using Business.UseCases.HomeworkSubmissions;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HomeworkSubmissionsController(
    SubmitHomeworkUseCase submitHomework,
    DeleteHomeworkSubmissionUseCase deleteSubmission,
    GradeHomeworkSubmissionUseCase gradeSubmission,
    ListHomeworkSubmissionUseCase listSubmissions,
    IStorageService storageService,
    IUserRepository userRepository
) : ControllerBase
{
    //Obtener ID de usuario (GUID) desde el token JWT
    private string CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

    // Buscar personaId asociado al usuario actual
    private async Task<int> GetCurrentPersonIdAsync()
    {
        if (string.IsNullOrEmpty(CurrentUserId) || CurrentUserId == "0")
            return 0;

        var user = await userRepository.FindByIdAsync(CurrentUserId);
        return user?.PersonId ?? 0;
    }

    private int CurrentUsuarioId => GetCurrentPersonIdAsync().Result;

    //Crear nueva entrega
    [HttpPost("Create")]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] SubmitHomeworkDto dto)
    {
        var result = await submitHomework.ExecuteAsync(CurrentUsuarioId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await listSubmissions.ExecuteAsync();
        return Ok(result);
    }

    // Actualizar entrega existente
    [HttpPut("Update/{homeworkId}")]
    [Authorize]
    public async Task<IActionResult> Update(int homeworkId, [FromForm] SubmitHomeworkDto dto)
    {
        dto.HomeworkId = homeworkId;

        var result = await submitHomework.ExecuteAsync(CurrentUsuarioId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    //Eliminar entrega
    [HttpDelete("Delete/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteSubmission.ExecuteAsync(id, CurrentUsuarioId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    // Calificar entrega
    [HttpPatch("Grade")]
    [Authorize]
    public async Task<IActionResult> Grade([FromBody] GradeHomeworkSubmissionDto dto)
    {
        var result = await gradeSubmission.ExecuteAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    //Obtener URL temporal SAS para ver archivo en Azure
    [HttpGet("GetSubmissionSasUrl/{submissionId}")]
    public async Task<IActionResult> GetSubmissionSasUrl(int submissionId)
    {

        var submission = await listSubmissions.GetByIdAsync(submissionId);
        if (submission == null)
        {
            return NotFound("No existe la entrega");
        }

        if (string.IsNullOrEmpty(submission.UrlArchivo))
        {
            return BadRequest("No hay archivo en esta entrega");
        }

        try
        {
            var url = await storageService.GetReadUrlAsync(
                submission.UrlArchivo,
                "submissions",
                TimeSpan.FromMinutes(30)
            );
            return Ok(new { url = url.ToString() });
        }
        catch (Exception ex)
        {
            return BadRequest("Error generando SAS: " + ex.Message);
        }
    }
}