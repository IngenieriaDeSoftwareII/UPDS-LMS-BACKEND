using Business.DTOs.Requests;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ChangePersonStatusUseCase(
    IPersonRepository repository, 
    IUserRepository userRepository)
{
    public async Task<Result<bool>> ExecuteAsync(int id, ChangePersonStatusDto dto, string? currentUserId)
    {
        var person = await repository.GetByIdAsync(id);
        
        if (person == null)
            return Result<bool>.Failure(["Persona no encontrada"]);

        // If the user is trying to deactivate themselves, block it
        if (!dto.IsActive && !string.IsNullOrEmpty(currentUserId))
        {
            var currentUser = await userRepository.FindByIdWithPersonAsync(currentUserId);
            if (currentUser != null && currentUser.PersonId == id)
                return Result<bool>.Failure(["No puedes desactivar tu propia información personal"]);
        }

        // If state is already the requested one
        if (person.IsActive == dto.IsActive)
            return Result<bool>.Success(true);

        // If trying to deactivate, check for active users
        if (!dto.IsActive)
        {
            var hasActiveUsers = await userRepository.HasActiveUsersAsync(id);
            if (hasActiveUsers)
                return Result<bool>.Failure(["No se puede desactivar la persona porque tiene cuentas de usuario activas asociadas. Por favor, desactive primero los usuarios."]);
        }

        person.IsActive = dto.IsActive;
        await repository.UpdateAsync(person);

        return Result<bool>.Success(true);
    }
}
