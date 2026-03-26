using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class UpdateMyProfileUseCase(
    IUserRepository userRepository,
    UpdatePersonUseCase updatePersonUseCase)
{
    public async Task<Result<PersonDto>> ExecuteAsync(string userId, UpdatePersonDto dto)
    {
        var user = await userRepository.FindByIdWithPersonAsync(userId);
        if (user is null)
            return Result<PersonDto>.Failure(["Identidad de usuario corrupta o no encontrada"]);

        // Invocar el caso de uso existente mandando el ID exacto de la persona amarrada al token.
        // Esto evita Vulneración de ID Directa (IDOR).
        return await updatePersonUseCase.ExecuteAsync(user.PersonId, dto);
    }
}
