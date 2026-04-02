using Business.DTOs.Requests;
using Business.UseCases.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController(
    CreateModuleUseCase createModule,
    ListModulesUseCase listModules,
    GetModuleByIdUseCase getModuleById,
    UpdateModuleUseCase updateModule,
    DeleteModuleUseCase deleteModule
) : ControllerBase
{
    // 🔹 CREATE
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
    {
        var result = await createModule.Execute(dto);
        return Ok(result);
    }

    // 🔹 GET ALL
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await listModules.Execute();
        return Ok(result);
    }

    // 🔹 GET BY ID
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await getModuleById.Execute(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // 🔹 UPDATE
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateModuleDto dto)
    {
        dto.Id = id;

        var success = await updateModule.Execute(dto);

        if (!success)
            return NotFound();

        return Ok();
    }

    // 🔹 DELETE
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await deleteModule.Execute(id);

        if (!success)
            return NotFound();

        return Ok();
    }
}