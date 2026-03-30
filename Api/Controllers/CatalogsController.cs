using Business.DTOs.Requests;
using Business.UseCases.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogsController(
    CreateCatalogUseCase createCatalogUseCase,
    ListCatalogsUseCase listCatalogsUseCase,
    GetCatalogByIdUseCase getCatalogByIdUseCase,
    UpdateCatalogUseCase updateCatalogUseCase,
    DeleteCatalogUseCase deleteCatalogUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listCatalogsUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await getCatalogByIdUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCatalogDto dto)
    {
        var result = await createCatalogUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateCatalogDto dto)
    {
        if (id != dto.Id) return BadRequest(new[] { "ID mismatch" });

        var result = await updateCatalogUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteCatalogUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return NoContent();
        return BadRequest(result.Errors);
    }
}