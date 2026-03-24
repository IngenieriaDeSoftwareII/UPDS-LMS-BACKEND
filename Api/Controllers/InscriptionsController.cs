using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsController(
    CreateInscriptionUseCase createInscription,
    ListInscriptionsUseCase listInscriptions) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInscriptionDto dto)
    {
        var result = await createInscription.ExecuteAsync(dto);
        if (!result.IsSuccess) return BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpGet("mis-cursos")]
    public async Task<IActionResult> GetMisCursos([FromQuery] int usuarioId)
    {
        var dto = new InscriptionByStudentDto { UsuarioId = usuarioId };
        var result = await listInscriptions.ExecuteAsync(dto);
        if (!result.IsSuccess) return BadRequest(result.Errors);
        return Ok(result.Value);
    }
}