using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(LoginUseCase login, RefreshTokenUseCase refreshToken) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await login.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return Unauthorized(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
    {
        var result = await refreshToken.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return Unauthorized(new { errors = result.Errors });

        return Ok(result.Value);
    }
}