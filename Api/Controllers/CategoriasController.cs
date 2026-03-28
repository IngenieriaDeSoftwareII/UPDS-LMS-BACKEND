using Business.DTOs.Requests;
using Business.UseCases.Categorias;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController(
    CreateCategoriaUseCase createCategoriaUseCase,
    ListCategoriasUseCase listCategoriasUseCase,
    GetCategoriaByIdUseCase getCategoriaByIdUseCase,
    UpdateCategoriaUseCase updateCategoriaUseCase,
    DeleteCategoriaUseCase deleteCategoriaUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listCategoriasUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await getCategoriaByIdUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCategoriaDto dto)
    {
        var result = await createCategoriaUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoriaDto dto)
    {
        if (id != dto.Id) return BadRequest("El ID de la ruta no coincide con el del cuerpo.");
        var result = await updateCategoriaUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteCategoriaUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return NoContent();
        return BadRequest(result.Errors);
    }
}
