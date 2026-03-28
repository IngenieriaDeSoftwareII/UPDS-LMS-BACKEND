using Business.DTOs.Requests;
using Business.UseCases.Catalogos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogosController(
    CreateCatalogoUseCase createCatalogoUseCase,
    ListCatalogosUseCase listCatalogosUseCase,
    GetCatalogoByIdUseCase getCatalogoByIdUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listCatalogosUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await getCatalogoByIdUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCatalogoDto dto)
    {
        var result = await createCatalogoUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
        return BadRequest(result.Errors);
    }
}