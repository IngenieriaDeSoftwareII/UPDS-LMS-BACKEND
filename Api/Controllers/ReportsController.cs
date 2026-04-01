using System.Security.Claims;
using Business.DTOs.Requests.Reports;
using Business.Services.Reports;
using Business.UseCases.Reports;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController(
    GetAdminCoursesReportUseCase getAdminCourses,
    GetAdminTeachersReportUseCase getAdminTeachers,
    GetTeacherSummaryReportUseCase getTeacherSummary,
    GetTeacherCoursesReportUseCase getTeacherCourses,
    GetTeacherCourseDetailReportUseCase getTeacherCourseDetail,
    IReportExportService exportService) : ControllerBase
{
    private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    private static bool TryParseFormat(string? format, out ReportExportFormat parsed)
    {
        parsed = ReportExportFormat.Xlsx;
        if (string.IsNullOrWhiteSpace(format))
            return false;

        return Enum.TryParse(format, true, out parsed);
    }

    [HttpGet("admin/courses")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAdminCourses([FromQuery] AdminCoursesReportQueryDto query)
    {
        var result = await getAdminCourses.ExecuteAsync(query);
        return Ok(result.Value);
    }

    [HttpGet("admin/courses/export")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> ExportAdminCourses([FromQuery] string format, [FromQuery] AdminCoursesReportQueryDto query)
    {
        if (!TryParseFormat(format, out var parsed))
            return BadRequest(new { errors = new[] { "Formato inválido. Usa 'xlsx' o 'pdf'." } });

        var result = await getAdminCourses.ExecuteAsync(query);
        var exported = exportService.ExportAdminCourses(result.Value!, parsed);
        return File(exported.Bytes, exported.ContentType, exported.FileName);
    }

    [HttpGet("admin/teachers")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAdminTeachers([FromQuery] AdminTeachersReportQueryDto query)
    {
        var result = await getAdminTeachers.ExecuteAsync(query);
        return Ok(result.Value);
    }

    [HttpGet("admin/teachers/export")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> ExportAdminTeachers([FromQuery] string format, [FromQuery] AdminTeachersReportQueryDto query)
    {
        if (!TryParseFormat(format, out var parsed))
            return BadRequest(new { errors = new[] { "Formato inválido. Usa 'xlsx' o 'pdf'." } });

        var result = await getAdminTeachers.ExecuteAsync(query);
        var exported = exportService.ExportAdminTeachers(result.Value!, parsed);
        return File(exported.Bytes, exported.ContentType, exported.FileName);
    }

    [HttpGet("teacher/summary")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> GetTeacherSummary([FromQuery] TeacherSummaryReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await getTeacherSummary.ExecuteAsync(CurrentUserId, query);
        return Ok(result.Value);
    }

    [HttpGet("teacher/summary/export")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> ExportTeacherSummary([FromQuery] string format, [FromQuery] TeacherSummaryReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        if (!TryParseFormat(format, out var parsed))
            return BadRequest(new { errors = new[] { "Formato inválido. Usa 'xlsx' o 'pdf'." } });

        var result = await getTeacherSummary.ExecuteAsync(CurrentUserId, query);
        var exported = exportService.ExportTeacherSummary(result.Value!, parsed);
        return File(exported.Bytes, exported.ContentType, exported.FileName);
    }

    [HttpGet("teacher/courses")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> GetTeacherCourses([FromQuery] TeacherCoursesReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await getTeacherCourses.ExecuteAsync(CurrentUserId, query);
        return Ok(result.Value);
    }

    [HttpGet("teacher/courses/export")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> ExportTeacherCourses([FromQuery] string format, [FromQuery] TeacherCoursesReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        if (!TryParseFormat(format, out var parsed))
            return BadRequest(new { errors = new[] { "Formato inválido. Usa 'xlsx' o 'pdf'." } });

        var result = await getTeacherCourses.ExecuteAsync(CurrentUserId, query);
        var exported = exportService.ExportTeacherCourses(result.Value!, parsed);
        return File(exported.Bytes, exported.ContentType, exported.FileName);
    }

    [HttpGet("teacher/courses/{courseId:int}")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> GetTeacherCourseDetail([FromRoute] int courseId, [FromQuery] TeacherCourseDetailReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await getTeacherCourseDetail.ExecuteAsync(CurrentUserId, courseId, query);
        return Ok(result.Value);
    }

    [HttpGet("teacher/courses/{courseId:int}/export")]
    [Authorize(Roles = UserRoles.Docente)]
    public async Task<IActionResult> ExportTeacherCourseDetail([FromRoute] int courseId, [FromQuery] string format, [FromQuery] TeacherCourseDetailReportQueryDto query)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        if (!TryParseFormat(format, out var parsed))
            return BadRequest(new { errors = new[] { "Formato inválido. Usa 'xlsx' o 'pdf'." } });

        var result = await getTeacherCourseDetail.ExecuteAsync(CurrentUserId, courseId, query);
        var exported = exportService.ExportTeacherCourseDetail(result.Value!, parsed);
        return File(exported.Bytes, exported.ContentType, exported.FileName);
    }
}

