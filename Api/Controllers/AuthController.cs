using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(
    LoginUseCase login,
    RefreshTokenUseCase refreshToken,
    LogoutUseCase logout,
    ChangePasswordUseCase changePassword) : ControllerBase
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenDto dto)
    {
        var result = await logout.ExecuteAsync(dto.RefreshToken);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = result.Value });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await changePassword.ExecuteAsync(userId, dto);
        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = "Contraseña actualizada exitosamente" });
    }
}