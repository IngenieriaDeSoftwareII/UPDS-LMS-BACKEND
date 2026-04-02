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
    public async Task<IActionResult> Create([FromBody] CreateInscriptionDto dto)
    {
        var result = await createInscription.ExecuteAsync(dto);

        if (!result.IsSuccess) 
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetMyCourses(int usuarioId)
    {
        var dto = new InscriptionByStudentDto { UsuarioId = usuarioId };

        var result = await listInscriptions.ExecuteAsync(dto);

        if (!result.IsSuccess) 
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpPatch("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelInscriptionDto dto)
    {
        var result = await cancelInscription.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Value);
    }
}