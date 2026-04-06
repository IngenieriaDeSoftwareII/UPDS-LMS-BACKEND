using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.HomeworkSubmissions;

public class DeleteHomeworkSubmissionUseCase(IHomeworkSubmissionRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id, int usuarioId)
    {
        var submission = await repository.GetByIdAsync(id);
        if (submission == null)
            return Result<bool>.Failure(["La entrega no existe."]);

        if (submission.UsuarioId != usuarioId)
            return Result<bool>.Failure(["No tienes permiso para eliminar esta entrega."]);

        if (submission.Revisado)
            return Result<bool>.Failure(["No puedes eliminar una entrega que ya ha sido calificada."]);

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}