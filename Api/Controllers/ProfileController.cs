using System.Security.Claims;
using Business.DTOs.Requests;
using Business.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController(
    GetMyProfileUseCase getProfile,
    UpdateMyProfileUseCase updateProfile) : ControllerBase
{
    private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await getProfile.ExecuteAsync(CurrentUserId);
        
        if (!result.IsSuccess)
            return NotFound(new { errors = result.Errors });

        return Ok(result.Value);
    }

    [HttpPatch("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdatePersonDto dto)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
            return Unauthorized();

        var result = await updateProfile.ExecuteAsync(CurrentUserId, dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(new
        {
            message = "Perfil actualizado correctamente",
            data = result.Value
        });
    }
}
