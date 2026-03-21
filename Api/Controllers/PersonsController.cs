using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController(CreatePersonUseCase createPerson, ListPersonsUseCase listPersons) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto dto)
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
}
