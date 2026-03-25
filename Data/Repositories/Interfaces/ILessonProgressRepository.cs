using System;
using System.Collections.Generic;
using System.Text;
using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ILessonProgressRepository
    {
        Task<LessonProgress> GetByUserAndLessonAsync(int usuarioId, int leccionId);
        Task<IEnumerable<LessonProgress>> GetByUserAndCourseAsync(int usuarioId, int cursoId);
        Task<LessonProgress> CreateAsync(LessonProgress progreso);
        Task<LessonProgress> UpdateAsync(LessonProgress progreso);
    }
}
