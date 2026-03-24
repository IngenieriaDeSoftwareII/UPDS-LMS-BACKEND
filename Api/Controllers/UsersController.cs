using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(CreateUserUseCase createUser) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var result = await createUser.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(Create), new { id = result.Value!.Id }, new
        {
            message = "Usuario registrado exitosamente",
            data = result.Value
        });
    }
}
