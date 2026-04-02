using System.Security.Claims;
using Business.DTOs.Requests;
using Business.UseCases;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EvaluationsController(
    CreateEvaluationUseCase createEvaluation,
    AddEvaluationQuestionUseCase addEvaluationQuestion,
    GetEvaluationToTakeUseCase getEvaluationToTake,
    SubmitEvaluationUseCase submitEvaluation,
    ListMyEvaluationGradesUseCase listMyEvaluationGrades,
    ListEvaluationGradesForTeacherUseCase listEvaluationGradesForTeacher,
    ListAvailableEvaluationsForStudentUseCase listAvailableEvaluations) : ControllerBase
{
    private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [Authorize(Roles = UserRoles.Docente)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEvaluationDto dto)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await createEvaluation.ExecuteAsync(CurrentUserId, dto);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Docente)]
    [HttpPost("questions")]
    public async Task<IActionResult> AddQuestion([FromBody] AddEvaluationQuestionDto dto)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await addEvaluationQuestion.ExecuteAsync(CurrentUserId, dto);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Estudiante)]
    [HttpGet("by-course/{cursoId:int}")]
    public async Task<IActionResult> GetByCourse(int cursoId)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await getEvaluationToTake.ExecuteAsync(CurrentUserId, cursoId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Estudiante)]
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitEvaluationDto dto)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await submitEvaluation.ExecuteAsync(CurrentUserId, dto);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Estudiante)]
    [HttpGet("my-grades")]
    public async Task<IActionResult> MyGrades()
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await listMyEvaluationGrades.ExecuteAsync(CurrentUserId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Estudiante)]
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableEvaluations()
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await listAvailableEvaluations.ExecuteAsync(CurrentUserId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [Authorize(Roles = UserRoles.Docente)]
    [HttpGet("by-course/{cursoId:int}/grades")]
    public async Task<IActionResult> GradesByCourse(int cursoId)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await listEvaluationGradesForTeacher.ExecuteAsync(CurrentUserId, cursoId);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }
}

