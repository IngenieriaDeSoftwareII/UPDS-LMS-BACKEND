using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsController(
    CreateInscriptionUseCase createInscription,
    ListInscriptionsUseCase listInscriptions,
    CancelInscriptionUseCase cancelInscription) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateInscriptionDto dto)
    {
        var result = await createInscription.ExecuteAsync(dto);

        if (!result.IsSuccess) 
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("mis-cursos")]
    public async Task<IActionResult> GetMyCourses(int usuarioId)
    {
        var dto = new InscriptionByStudentDto { UsuarioId = usuarioId };

        var result = await listInscriptions.ExecuteAsync(dto);

        if (!result.IsSuccess) 
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("cancelar")]
    public async Task<IActionResult> Cancel(CancelInscriptionDto dto)
    {
        var result = await cancelInscription.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}