using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
    public async Task<Course> CreateAsync(Course course)
    {
        context.Courses.Add(course);
        await context.SaveChangesAsync();
        return course;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await context.Courses
            .Include(c => c.Category)
            .Include(c => c.Teacher)
            .Where(c => c.EntityStatus == 1).ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await context.Courses
            .Include(c => c.Category)
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Course course)
    {
        course.UpdatedAt = DateTime.UtcNow;
        context.Courses.Update(course);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var course = await GetByIdAsync(id);
        if (course != null)
        {
            course.EntityStatus = 0; // Soft delete
            course.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }
}