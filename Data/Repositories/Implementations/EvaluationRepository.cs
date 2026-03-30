using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class EvaluationRepository(AppDbContext dbContext) : IEvaluationRepository
{
    public async Task<Evaluation> CreateAsync(Evaluation evaluation)
    {
        dbContext.Evaluations.Add(evaluation);
        await dbContext.SaveChangesAsync();
        return evaluation;
    }

    public async Task<Question> AddQuestionAsync(Question question, IEnumerable<AnswerOption> options)
    {
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var optionsList = options.ToList();
        foreach (var option in optionsList)
            option.PreguntaId = question.Id;

        dbContext.AnswerOptions.AddRange(optionsList);
        await dbContext.SaveChangesAsync();
        return question;
    }

    public Task<Evaluation?> GetByIdAsync(int id) =>
        dbContext.Evaluations.FirstOrDefaultAsync(e => e.Id == id);

    public Task<Evaluation?> GetByIdWithQuestionsAsync(int id) =>
        dbContext.Evaluations
            .Include(e => e.Preguntas.OrderBy(p => p.Orden))
            .ThenInclude(p => p.OpcionesRespuesta.OrderBy(o => o.Orden))
            .FirstOrDefaultAsync(e => e.Id == id);

    public Task<Evaluation?> GetByCourseIdWithQuestionsAsync(int cursoId) =>
        dbContext.Evaluations
            .Include(e => e.Preguntas.OrderBy(p => p.Orden))
            .ThenInclude(p => p.OpcionesRespuesta.OrderBy(o => o.Orden))
            .FirstOrDefaultAsync(e => e.CursoId == cursoId);

    public Task<int> CountAttemptsAsync(int evaluacionId, int usuarioId) =>
        dbContext.EvaluationAttempts
            .Where(a => a.EvaluacionId == evaluacionId && a.UsuarioId == usuarioId)
            .CountAsync();

    public async Task<EvaluationAttempt> CreateAttemptAsync(EvaluationAttempt attempt, IEnumerable<EvaluationAnswer> answers)
    {
        dbContext.EvaluationAttempts.Add(attempt);
        await dbContext.SaveChangesAsync();

        var answersList = answers.ToList();
        foreach (var answer in answersList)
            answer.IntentoId = attempt.Id;

        dbContext.EvaluationAnswers.AddRange(answersList);
        await dbContext.SaveChangesAsync();

        return attempt;
    }

    public async Task<IEnumerable<EvaluationAttempt>> GetAttemptsByStudentAsync(int usuarioId)
    {
        return await dbContext.EvaluationAttempts
            .Where(a => a.UsuarioId == usuarioId)
            .Include(a => a.Evaluaciones)
                .ThenInclude(e => e.Preguntas)
                    .ThenInclude(q => q.OpcionesRespuesta)
            .Include(a => a.Respuestas)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<EvaluationAttempt>> GetAttemptsByEvaluationAsync(int evaluacionId)
    {
        return await dbContext.EvaluationAttempts
            .Where(a => a.EvaluacionId == evaluacionId)
            .Include(a => a.Evaluaciones)
                .ThenInclude(e => e.Preguntas)
                    .ThenInclude(q => q.OpcionesRespuesta)
            .Include(a => a.Respuestas)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}

