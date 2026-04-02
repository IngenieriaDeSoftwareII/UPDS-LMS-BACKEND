using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases;

public class ListAvailableEvaluationsForStudentUseCase(IUserRepository userRepository, IInscriptionRepository inscriptionRepository, IMapper mapper)
{
    public async Task<Result<IEnumerable<CourseDto>>> ExecuteAsync(string userId)
    {
        var user = await userRepository.FindByIdAsync(userId);
        if (user == null)
            return Result<IEnumerable<CourseDto>>.Failure(["Usuario no encontrado"]);

        var courses = await inscriptionRepository.GetCoursesWithEvaluationsByUserAsync(user.PersonId);
        return Result<IEnumerable<CourseDto>>.Success(mapper.Map<IEnumerable<CourseDto>>(courses));
    }
}