using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PersonsController(
    CreatePersonUseCase createPerson, 
    ListPersonsUseCase listPersons,
    UpdatePersonUseCase updatePerson) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var result = await createPerson.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await listPersons.ExecuteAsync();
        return Ok(result);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonDto dto)
    {
        var result = await updatePerson.ExecuteAsync(id, dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = "Datos de persona actualizados correctamente", data = result.Value });
    }
}
