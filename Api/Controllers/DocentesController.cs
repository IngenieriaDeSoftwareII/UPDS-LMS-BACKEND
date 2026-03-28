using Business.DTOs.Requests;
using Business.UseCases.Docentes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocentesController(
    CreateDocenteUseCase createDocenteUseCase,
    ListDocentesUseCase listDocentesUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listDocentesUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDocenteDto dto)
    {
        var result = await createDocenteUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }
}
