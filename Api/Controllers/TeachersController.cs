using Business.DTOs.Requests;
using Business.UseCases.Teacher;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(
    CreateTeacherUseCase createUseCase,
    ListTeachersUseCase listUseCase,
    GetTeacherByIdUseCase getByIdUseCase,
    UpdateTeacherUseCase updateUseCase,
    DeleteTeacherUseCase deleteUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await listUseCase.ExecuteAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await getByIdUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTeacherDto dto)
    {
        var result = await createUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTeacherDto dto)
    {
        if (id != dto.Id) return BadRequest("Route ID does not match Body ID.");
        var result = await updateUseCase.ExecuteAsync(dto);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteUseCase.ExecuteAsync(id);
        if (result.IsSuccess) return NoContent();
        return BadRequest(result.Errors);
    }
}