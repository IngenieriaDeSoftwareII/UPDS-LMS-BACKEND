using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsController(
    CreateInscription createInscription,
    GetMyCourses getMyCourses) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InscriptionCreateRequest request)
    {
        var result = await createInscription.ExecuteAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpGet("mis-cursos")]
    public async Task<IActionResult> GetMisCursos([FromQuery] int usuarioId)
    {
        var result = await getMyCourses.ExecuteAsync(usuarioId);
        if (!result.IsSuccess) return BadRequest(result.Errors);
        return Ok(result.Value);
    }
}