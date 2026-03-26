using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class GetMyProfileUseCase(IUserRepository userRepository)
{
    public async Task<Result<ProfileDto>> ExecuteAsync(string userId)
    {
        var user = await userRepository.FindByIdWithPersonAsync(userId);
        if (user is null)
            return Result<ProfileDto>.Failure(["Usuario no encontrado"]);

        var roles = await userRepository.GetRolesAsync(user);

        return Result<ProfileDto>.Success(new ProfileDto
        {
            Id = user.Id,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? string.Empty,
            
            PersonId = user.PersonId,
            FirstName = user.Person.FirstName,
            LastName = user.Person.LastName,
            MotherLastName = user.Person.MotherLastName,
            DateOfBirth = user.Person.DateOfBirth,
            Gender = user.Person.Gender,
            NationalId = user.Person.NationalId,
            NationalIdExpedition = user.Person.NationalIdExpedition,
            PhoneNumber = user.Person.PhoneNumber,
            Address = user.Person.Address
        });
    }
}
