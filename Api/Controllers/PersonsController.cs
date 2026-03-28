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
    UpdatePersonUseCase updatePerson,
    ChangePersonStatusUseCase changePersonStatus) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var result = await createPerson.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetActive), new { id = result.Value!.Id }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var result = await listPersons.ExecuteAsync();
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listPersons.ExecuteAsync(includeInactive: true);
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

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangePersonStatusDto dto)
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var result = await changePersonStatus.ExecuteAsync(id, dto, currentUserId);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        var message = dto.IsActive ? "Persona reactivada correctamente" : "Persona desactivada correctamente";
        return Ok(new { message });
    }
}
