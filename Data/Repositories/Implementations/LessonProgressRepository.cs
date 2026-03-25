using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations
{
    public class LessonProgressRepository(AppDbContext context) : ILessonProgressRepository
    {
        public async Task<LessonProgress?> GetByUserAndLessonAsync(int usuario_id, int leccion_id)
        {
            return await context.LessonProgresses
                .FirstOrDefaultAsync(p => p.usuario_id == usuario_id && p.leccion_id == leccion_id);
        }

        public async Task<IEnumerable<LessonProgress>> GetByUserAndCourseAsync(int usuario_id, int curso_id)
        {
            return await context.LessonProgresses
                .Include(p => p.Lesson)
                    .ThenInclude(l => l.Module)
                .Where(p => p.usuario_id == usuario_id && p.Lesson.Module.curso_id == curso_id)
                .ToListAsync();
        }

        public async Task<LessonProgress> CreateAsync(LessonProgress progreso)
        {
            context.LessonProgresses.Add(progreso);
            await context.SaveChangesAsync();
            return progreso;
        }

        public async Task<LessonProgress> UpdateAsync(LessonProgress progreso)
        {
            context.LessonProgresses.Update(progreso);
            await context.SaveChangesAsync();
            return progreso;
        }
    }
}
