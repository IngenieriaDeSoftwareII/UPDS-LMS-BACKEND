using System.Security.Claims;
using Business.UseCases.StudentProgress;
using Data.Enums;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/student/progress")]
[Authorize(Roles = UserRoles.Estudiante)]
public class StudentProgressController(
    IUserRepository userRepository,
    GetStudentCourseLearningUseCase getStudentCourseLearning,
    CompleteLessonUseCase completeLesson,
    GetStudentProgressDashboardUseCase getStudentProgressDashboard,
    GetModuleWeightedGradesUseCase getModuleWeightedGrades,
    GenerateCourseCertificatePdfUseCase generateCourseCertificatePdf) : ControllerBase
{
    private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [HttpGet("courses/{cursoId:int}/learning")]
    public async Task<IActionResult> GetCourseLearning(int cursoId)
    {
        var user = await ResolvePersonIdAsync();
        if (user is null) return Unauthorized();

        var result = await getStudentCourseLearning.ExecuteAsync(user.Value, cursoId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpPost("lessons/{leccionId:int}/complete")]
    public async Task<IActionResult> CompleteLesson(int leccionId)
    {
        var user = await ResolvePersonIdAsync();
        if (user is null) return Unauthorized();

        var result = await completeLesson.ExecuteAsync(user.Value, leccionId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var user = await ResolvePersonIdAsync();
        if (user is null) return Unauthorized();

        var result = await getStudentProgressDashboard.ExecuteAsync(user.Value);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpGet("courses/{cursoId:int}/module-grades")]
    public async Task<IActionResult> ModuleGrades(int cursoId)
    {
        var user = await ResolvePersonIdAsync();
        if (user is null) return Unauthorized();

        var result = await getModuleWeightedGrades.ExecuteAsync(user.Value, cursoId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpGet("courses/{cursoId:int}/certificate")]
    public async Task<IActionResult> Certificate(int cursoId)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await generateCourseCertificatePdf.ExecuteAsync(CurrentUserId, cursoId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return File(result.Value!, "application/pdf", $"certificado-curso-{cursoId}.pdf");
    }

    private async Task<int?> ResolvePersonIdAsync()
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return null;

        var user = await userRepository.FindByIdWithPersonAsync(CurrentUserId);
        return user?.Person?.Id;
    }
}
