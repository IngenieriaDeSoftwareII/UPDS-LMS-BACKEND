using Business.DTOs.Requests;
using Business.UseCases.Cursos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController(
    CreateCursoUseCase createCursoUseCase,
    ListCursosUseCase listCursosUseCase,
    GetCursoByIdUseCase getCursoByIdUseCase,
    UpdateCursoUseCase updateCursoUseCase,
    DeleteCursoUseCase deleteCursoUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listCursosUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await getCursoByIdUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCursoDto dto)
    {
        var result = await createCursoUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateCursoDto dto)
    {
        if (id != dto.Id) return BadRequest("El ID de la ruta no coincide con el del cuerpo.");
        var result = await updateCursoUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteCursoUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return NoContent();
        return BadRequest(result.Errors);
    }
}
