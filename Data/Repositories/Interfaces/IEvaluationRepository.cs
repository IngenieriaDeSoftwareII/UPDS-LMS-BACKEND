using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IEvaluationRepository
{
    Task<Evaluation> CreateAsync(Evaluation evaluation);
    Task<Question> AddQuestionAsync(Question question, IEnumerable<AnswerOption> options);
    Task<Evaluation?> GetByIdAsync(int id);
    Task<Evaluation?> GetByIdWithQuestionsAsync(int id);
    Task<Evaluation?> GetByCourseIdWithQuestionsAsync(int cursoId);
    Task<int> CountAttemptsAsync(int evaluacionId, int usuarioId);
    Task<EvaluationAttempt> CreateAttemptAsync(EvaluationAttempt attempt, IEnumerable<EvaluationAnswer> answers);
    Task<IEnumerable<EvaluationAttempt>> GetAttemptsByStudentAsync(int usuarioId);
    Task<IEnumerable<EvaluationAttempt>> GetAttemptsByEvaluationAsync(int evaluacionId);
    Task<(Evaluation? Evaluation, EvaluationAttempt? BestAttempt)> GetCourseEvaluationAndBestAttemptAsync(
        int cursoId,
        int personId);
}

