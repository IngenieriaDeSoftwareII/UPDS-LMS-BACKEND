using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using FluentValidation;

namespace Business.UseCases.HomeworkSubmissions;

public class SubmitHomeworkUseCase(
    IHomeworkRepository homeworkRepository,
    IHomeworkSubmissionRepository submissionRepository,
    IMapper mapper,
    IValidator<SubmitHomeworkDto> validator,
    IStorageService storageService)
{
    public async Task<Result<HomeworkSubmissionDto>> ExecuteAsync(int usuarioId, SubmitHomeworkDto dto)
    {
        // 🔹 VALIDACIÓN
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<HomeworkSubmissionDto>.Failure(
                validation.Errors.Select(e => e.ErrorMessage)
            );

        // 🔹 VERIFICAR TAREA
        var homework = await homeworkRepository.GetByIdAsync(dto.HomeworkId);
        if (homework == null)
            return Result<HomeworkSubmissionDto>.Failure(["La tarea no existe."]);

        var now = DateTime.Now;

        // 🔹 VALIDACIONES DE FECHA
        if (now < homework.FechaApertura)
            return Result<HomeworkSubmissionDto>.Failure(["La tarea aún no está disponible."]);

        if (homework.FechaLimite.HasValue && now > homework.FechaLimite.Value)
            return Result<HomeworkSubmissionDto>.Failure(["La fecha límite ha expirado."]);

        // 🔹 ESTADO
        string estado = now > homework.FechaEntrega ? "tarde" : "entregado";

        // 🔹 VERIFICAR SI YA EXISTE ENTREGA
        var existing = await submissionRepository
            .GetByUserAndHomeworkAsync(dto.HomeworkId, usuarioId);

        if (existing != null && existing.Revisado)
            return Result<HomeworkSubmissionDto>.Failure(
                ["No puedes editar una entrega ya calificada."]
            );

        var submission = existing ?? new HomeworkSubmission();

        // 🔥 SUBIR ARCHIVO (CORRECTO)
        if (dto.File != null)
        {
            // eliminar anterior si existe
            if (existing != null && !string.IsNullOrEmpty(existing.UrlArchivo))
            {
                await storageService.DeleteFileAsync(existing.UrlArchivo, "submissions");
            }

            using var stream = dto.File.OpenReadStream();

            var fullUrl = await storageService.UploadFileAsync(
                stream,
                dto.File.FileName,
                "submissions"
            );
            submission.UrlArchivo = Path.GetFileName(fullUrl);

            submission.TamanoKb = (int)(dto.File.Length / 1024);

            submission.Formato = Enum.Parse<FormatDocument>(
                Path.GetExtension(dto.File.FileName).TrimStart('.'),
                true
            );
        }
        else if (existing == null)
        {
            return Result<HomeworkSubmissionDto>.Failure(
                ["Debes subir un archivo."]
            );
        }

        // 🔹 DATOS GENERALES
        submission.HomeworkId = dto.HomeworkId;
        submission.UsuarioId = usuarioId;
        submission.Comentario = dto.Comentario;
        submission.FechaEntrega = now;
        submission.Estado = estado;
        submission.UpdatedAt = now;

        // 🔹 GUARDAR
        var result = existing != null
            ? await submissionRepository.UpdateAsync(submission)
            : await submissionRepository.CreateAsync(submission);

        return Result<HomeworkSubmissionDto>.Success(
            mapper.Map<HomeworkSubmissionDto>(result)
        );
    }
}