using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<Course> CreateAsync(Course course);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetByIdWithModulesLessonsAndTeacherAsync(int id);
    Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId);
    Task<IEnumerable<Course>> GetByTeacherIdWithoutEvaluationAsync(int teacherId);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
}

