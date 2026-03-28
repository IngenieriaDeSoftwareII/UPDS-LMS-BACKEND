using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations
{
    public class CourseRepository(AppDbContext dbContext) : ICourseRepository
    {
        public Task<Course?> GetByIdAsync(int id) =>
        dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
}
