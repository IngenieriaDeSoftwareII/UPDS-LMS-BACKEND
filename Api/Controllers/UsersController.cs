using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(CreateUserUseCase createUser, ListUsersUseCase listUsers) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var result = await createUser.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, new
        {
            message = "Usuario registrado exitosamente",
            data = result.Value
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null)
    {
        var users = await listUsers.ExecuteAsync(search);
        return Ok(users);
    }
}
